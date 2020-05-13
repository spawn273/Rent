using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;

namespace RentApi.Api.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AccountsController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public AccountsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<AccountDTO[]>> GetList(
            int _start = 0, int _end = 10,
            string _sort = "id", string _order = "ASC",
            int? shopId = null,
            string q = "")
        {
            var query = _context.Users.AsQueryable();

            if (shopId.HasValue)
            {
                query = query.Where(x => x.Employee.ShopId == shopId);
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => 
                    EF.Functions.ILike(x.FirstName, $"%{q}%") ||
                    EF.Functions.ILike(x.MiddleName, $"%{q}%") ||
                    EF.Functions.ILike(x.LastName, $"%{q}%") ||
                    EF.Functions.ILike(x.UserName, $"%{q}%")
                );
            }

            var count = await query.CountAsync();
            SetTotalCount(count);

            // TODO: sort
            var result = await query
                .Skip(_start)
                .Take(_end - _start)
                .ToDTO()
                .ToArrayAsync();

            foreach (var account in result)
            {
                account.Role = (await _userManager.GetRolesAsync(await _context.Users.FindAsync(account.Id))).First();
            }

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> Get(int id)
        {
            var entity = await _context.Users
                .Where(x => x.Id == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            entity.Role = (await _userManager.GetRolesAsync(await _context.Users.FindAsync(id))).First();

            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDTO>> Put(int id, AccountDTO dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var user = await _context.Users
                        .Include(x => x.Employee)
                        .FirstOrDefaultAsync(x => x.Id == id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    var names = dto.Name.Split(' ');
                    user.FirstName = names[0];
                    user.MiddleName = names[1];
                    user.LastName = names[2];
                    user.UserName = dto.UserName;

                    var oldRole = (await _userManager.GetRolesAsync(user)).First();
                    var newRole = dto.Role;
                    if (oldRole != newRole)
                    {
                        if (oldRole == "admin")
                        {
                            // admin -> employee
                            var employee = new Employee
                            {
                                ShopId = dto.ShopId.Value,
                                UserId = user.Id
                            };
                            _context.Employee.Add(employee);
                        }
                        else
                        {
                            // employee -> admin
                            var employee = await _context.Users.Where(x => x.Id == dto.Id).Select(x => x.Employee).FirstAsync();
                            _context.Employee.Remove(employee);
                        }

                        var currentRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRoleAsync(user, oldRole);
                        await _userManager.AddToRoleAsync(user, newRole);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                }
                catch (Exception)
                {
                    throw;
                }

                return dto;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Post(AccountDTO dto)
        {
            var names = dto.Name.Split(' ');
            var user = new User
            {
                UserName = dto.UserName,
                FirstName = names[0],
                MiddleName = names[1],
                LastName = names[2],
            };

            // TODO: validate input

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userResult = await _userManager.CreateAsync(user, dto.Password);
                    if (!userResult.Succeeded)
                    {
                        return BadRequest(userResult.Errors);
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, dto.Role);
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(roleResult.Errors);
                    }

                    if (dto.Role == "emloyee")
                    {
                        var emloyee = new Employee
                        {
                            UserId = user.Id,
                            ShopId = dto.ShopId.Value
                        };
                        _context.Employee.Add(emloyee);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction("Get", new { id = user.Id }, user);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteCustomer(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
