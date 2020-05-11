using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.Extensions;
using RentApi.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RentApi.Api.Guest
{
    [Route("api/guest/[controller]")]
    [ApiController]
    public class EquipmentController : BaseApiController
    {
        private readonly AppDbContext _context;

        public EquipmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetEquipmentList(
            int _start = 0, int _end = 10,
            string _sort = "id", string _order = "ASC",
            int? shopId = null, int? equipmentType = null)
        {
            var query = _context.Equipment.AsQueryable();

            if (shopId.HasValue)
            {
                query = query.Where(x => x.ShopId == shopId);
            }

            if (equipmentType.HasValue)
            {
                query = query.Where(x => x.EquipmentTypeId == equipmentType);
            }

            var count = await query.CountAsync();
            SetTotalCount(count);

            var result = await query
                .Skip(_start)
                .Take(_end - _start)
                .OrderBy($"{_sort} {_order}")
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("many")]
        public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetMany([FromQuery] int[] id)
        {
            var result = await _context.Equipment
                .Where(x => id.Contains(x.Id))
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentDTO>> GetEquipment(int id)
        {
            var equipment = await _context.Equipment
                .Where(x => x.Id == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }
    }
}
