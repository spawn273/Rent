using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Api.Extensions;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetList(
            int _start = 0, int _end = 10,
            string _sort = "id", string _order = "ASC",
            int? shopId = null)
        {
            var query = _context.Employee.AsQueryable().Where(x => !x.User.Deleted);

            if (shopId.HasValue)
            {
                query = query.Where(x => x.ShopId == shopId);
            }

            var count = await query.CountAsync();
            HttpContext.SetTotalCount(count);

            var result = await query
                .ToDTO()
                .OrderBy($"{_sort} {_order}")
                .Skip(_start)
                .Take(_end - _start)
                .ToArrayAsync();

            return result;
        }

        [HttpGet("many")]
        public async Task<ActionResult<EmployeeDTO[]>> GetMany([FromQuery] int[] id)
        {
            var result = await _context.Employee
                .Where(x => id.Contains(x.Id))
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(int id)
        {
            var employee = await _context.Employee
                .Where(x => x.Id == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }
    }
}
