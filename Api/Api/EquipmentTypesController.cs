using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Api.Extensions;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using System.Collections.Generic;
using System.Linq;
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

        // GET: api/EquipmentTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentType>>> GetEquipmentType()
        {
            Response.Headers.Add("X-Total-Count", _context.EquipmentType.Count().ToString());
            return await _context.EquipmentType.ToListAsync();
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

        // GET: api/EquipmentTypes/5
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
        public async Task<ActionResult<EquipmentTypeDTO>> PutEquipmentType(int id, EquipmentTypeDTO equipmentType)
        {
            var entity = await _context.EquipmentType.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = equipmentType.Name;
            entity.PricePerDay = equipmentType.PricePerDay;
            entity.PricePerHour = equipmentType.PricePerHour;

            await _context.SaveChangesAsync();

            return equipmentType;
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

            _context.EquipmentType.Remove(equipmentType);
            await _context.SaveChangesAsync();

            return equipmentType;
        }
    }
}
