using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Api.Extensions;
using RentApi.Application;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<RoleDTO[]>> Get()
        {
            var roles = await _context.Roles.ToDTO().ToArrayAsync();
            HttpContext.SetTotalCount(roles.Length);
            return roles;
        }

        [HttpGet("many")]
        public async Task<ActionResult<RoleDTO[]>> GetMany([FromQuery] string[] id)
        {
            var result = await _context.Roles
                .Where(x => id.Contains(x.Name))
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> Get(string id)
        {
            var role = await _context.Roles.Where(x => x.Name == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }
    }
}
