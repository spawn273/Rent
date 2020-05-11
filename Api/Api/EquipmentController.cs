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
using SmartAnalytics.BASF.Backend.Infrastructure;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : BaseApiController
    {
        private readonly AppDbContext _context;

        public EquipmentController(AppDbContext context)
        {
            _context = context;
        }

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


        [HttpPut("{id}")]
        public async Task<ActionResult<EquipmentDTO>> PutEquipment(int id, EquipmentDTO equipment)
        {
            var entity = await _context.Equipment.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = equipment.Name;
            entity.EquipmentTypeId = equipment.EquipmentTypeId;

            await _context.SaveChangesAsync();

            return equipment;
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentDTO>> PostEquipment(EquipmentDTO dto)
        {
            var equipment = new Equipment
            {
                ShopId = HttpContext.GetShopId(),
                EquipmentTypeId = dto.EquipmentTypeId,
                Name = dto.Name
            };
            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();

            dto.Id = equipment.Id;

            return CreatedAtAction("GetEquipment", new { id = equipment.Id }, dto);
        }

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
    }
}
