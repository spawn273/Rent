using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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
            var query = _context.Users.AsQueryable().Where(x => !x.Deleted);

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

            if (_sort == "name")
            {
                _sort = "firstName";
            }

            var dtos = (await query.Include(x => x.Employee).ToListAsync()).AsQueryable()
                .ToDTO()
                .ToList();
            foreach (var dto in dtos)
            {
                dto.RoleId = (await _userManager.GetRolesAsync(await _context.Users.FindAsync(dto.Id))).First();
            }
            
            var result = dtos.AsQueryable()
                .OrderBy($"{_sort} {_order}")
                .Skip(_start)
                .Take(_end - _start)
                .ToArray();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDTO>> Get(int id)
        {
            var entity = await _context.Users
                .Where(x => x.Id == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            entity.RoleId = (await _userManager.GetRolesAsync(await _context.Users.FindAsync(id))).First();

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

                    user.FirstName = dto.FirstName;
                    user.MiddleName = dto.MiddleName;
                    user.LastName = dto.LastName;
                    user.UserName = dto.UserName;
                    user.Employee.ShopId = dto.ShopId;

                    var oldRole = (await _userManager.GetRolesAsync(user)).First();
                    var newRole = dto.RoleId;
                    if (oldRole != newRole)
                    {
                        var currentRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRoleAsync(user, oldRole);
                        await _userManager.AddToRoleAsync(user, newRole);
                    }

                    await _userManager.UpdateAsync(user);
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
        public async Task<ActionResult<AccountDTO>> Post(AccountDTO dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                EmailConfirmed = true
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

                    var roleResult = await _userManager.AddToRoleAsync(user, dto.RoleId);
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(roleResult.Errors);
                    }

                    var emloyee = new Employee
                    {
                        UserId = user.Id,
                        ShopId = dto.ShopId
                    };
                    _context.Employee.Add(emloyee);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    dto.Id = user.Id;
                    return dto;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = $"{user.UserName}_deleted";
            user.Deleted = true;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
