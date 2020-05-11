using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Api.Extensions;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseApiController
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {
            var result = await _context.Employee.ToDTO().ToArrayAsync();
            SetTotalCount(_context.Employee.Count());
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

        // GET: api/Employees/5
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

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDTO>> PutEmployee(int id, EmployeeDTO dto)
        {
            var entity = await _context.Employee.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult> PostEmployee(EmployeeDTO employee)
        {
            return Ok();
            //_context.Employee.Add(employee);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return employee;
        }
    }
}
