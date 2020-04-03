using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<EquipmentType>>> GetMany([FromQuery] int[] id)
        {
            var result = await _context.EquipmentType
                .Where(x => id.Contains(x.Id))
                .ToArrayAsync();

            return result;
        }

        // GET: api/EquipmentTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentType>> GetEquipmentType(int id)
        {
            var equipmentType = await _context.EquipmentType.FindAsync(id);

            if (equipmentType == null)
            {
                return NotFound();
            }

            return equipmentType;
        }

        // PUT: api/EquipmentTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentType(int id, EquipmentType equipmentType)
        {
            if (id != equipmentType.Id)
            {
                return BadRequest();
            }

            _context.Entry(equipmentType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EquipmentTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<EquipmentType>> PostEquipmentType(EquipmentType equipmentType)
        {
            _context.EquipmentType.Add(equipmentType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipmentType", new { id = equipmentType.Id }, equipmentType);
        }

        // DELETE: api/EquipmentTypes/5
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

        private bool EquipmentTypeExists(int id)
        {
            return _context.EquipmentType.Any(e => e.Id == id);
        }
    }
}
