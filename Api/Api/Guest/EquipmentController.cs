using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
            int? shop = null, int? equipmentType = null, bool available = false,
            string q = "")
        {
            var query = _context.Equipment.AsQueryable();

            if (shop.HasValue)
            {
                query = query.Where(x => x.ShopId == shop);
            }

            if (equipmentType.HasValue)
            {
                query = query.Where(x => x.EquipmentTypeId == equipmentType);
            }

            if (available)
            {
                query = query.Where(x => !x.RentEquipment.Any());
            }

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{q}%"));
            }

            var count = await query.CountAsync();
            SetTotalCount(count);

            IQueryable<EquipmentDTO> ordered;
            if (_sort == "available")
            {
                // Sort by DTO
                ordered = query
                    .ToDTO()
                    .OrderBy($"{_sort} {_order}");
            } 
            else
            {
                // Sort by Entity                
                var order = $"{_sort} {_order}";
                if (_sort == "type")
                {
                    order = $"equipmentType.name {_order}";
                }
                ordered = query
                    .OrderBy(order)
                    .ToDTO();
            }

            var result = await ordered
                .Skip(_start)
                .Take(_end - _start)
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
