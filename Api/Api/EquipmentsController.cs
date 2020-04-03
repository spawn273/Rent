using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Api.Extensions;
using RentApi.Infrastructure;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : BaseApiController
    {
        private readonly AppDbContext _context;

        public EquipmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetEquipmentList(int? shopId, int? rentId)
        {
            var query = _context.Equipment.AsQueryable();

            if (shopId.HasValue)
            {
                query = query.Where(x => x.ShopId == shopId);
            }

            if (rentId.HasValue)
            {
                query = _context.RentEquipment
                    .Where(x => x.RentId == rentId)
                    .Select(x => x.Equipment);
            }

            var equipment = await query.ToDTO().ToArrayAsync();
            SetTotalCount(equipment.Length);
            return equipment;
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

        // GET: api/Equipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentDTO>> GetEquipment(int id)
        {
            var equipment = await _context.Equipment.Where(x => x.Id == id).ToDTO().FirstOrDefaultAsync();

            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // PUT: api/Equipments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ActionResult<Equipment>> PutEquipment(int id, Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return BadRequest();
            }

            _context.Entry(equipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return equipment;
            //return NoContent();
        }

        // POST: api/Equipments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipment", new { id = equipment.Id }, equipment);
        }

        // DELETE: api/Equipments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Equipment>> DeleteEquipment(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }

            _context.Equipment.Remove(equipment);
            await _context.SaveChangesAsync();

            return equipment;
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipment.Any(e => e.Id == id);
        }
    }
}
