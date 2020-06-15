using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Api.Extensions;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipmentTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<EquipmentTypeDTO[]>> GetList(
            int _start = 0, int _end = 10,
            string _sort = "id", string _order = "ASC",
            string q = "")
        {
            var query = _context.EquipmentType.AsQueryable().Where(x => !x.Archived);

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => EF.Functions.ILike(x.Name, $"%{q}%"));
            }

            var count = await query.CountAsync();
            HttpContext.SetTotalCount(count);

            var result = await query
                .OrderBy($"{_sort} {_order}")
                .Skip(_start)
                .Take(_end - _start)
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("many")]
        public async Task<ActionResult<IEnumerable<EquipmentTypeDTO>>> GetMany([FromQuery] int[] id)
        {
            var result = await _context.EquipmentType
                .Where(x => id.Contains(x.Id))
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentTypeDTO>> GetEquipmentType(int id)
        {
            var equipmentType = await _context.EquipmentType.Where(x => x.Id == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            if (equipmentType == null)
            {
                return NotFound();
            }

            return equipmentType;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EquipmentTypeDTO>> PutEquipmentType(int id, EquipmentTypeDTO dto)
        {
            var entity = await _context.EquipmentType.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = dto.Name;
            entity.PricePerDay = dto.PricePerDay;
            entity.PricePerHour = dto.PricePerHour;

            await _context.SaveChangesAsync();

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentTypeDTO>> PostEquipmentType(EquipmentTypeDTO dto)
        {
            var equipmentType = new EquipmentType
            {
                Name = dto.Name,
                PricePerDay = dto.PricePerDay,
                PricePerHour = dto.PricePerHour
            };

            _context.EquipmentType.Add(equipmentType);
            await _context.SaveChangesAsync();

            dto.Id = equipmentType.Id;

            return CreatedAtAction("GetEquipmentType", new { id = equipmentType.Id }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EquipmentType>> DeleteEquipmentType(int id)
        {
            var equipmentType = await _context.EquipmentType.FindAsync(id);
            if (equipmentType == null)
            {
                return NotFound();
            }

            equipmentType.Archived = true;
            await _context.SaveChangesAsync();

            return equipmentType;
        }
    }
}
