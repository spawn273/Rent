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
using RentApi.Infrastructure;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EquipmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentDTO>>> GetList(
            int _start = 0, int _end = 10,
            string _sort = "id", string _order = "ASC",
            int? shopId = null, int? rentId = null,
            string q = "")
        {
            var query = _context.Equipment.AsQueryable().Where(x => !x.Archived);

            if (shopId.HasValue)
            {
                query = query.Where(x => x.ShopId == shopId);
            }

            //if (rentId.HasValue)
            //{
            //    query = _context.RentEquipment
            //        .Where(x => x.RentId == rentId)
            //        .Select(x => x.Equipment);
            //}

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => 
                    EF.Functions.ILike(x.Id.ToString(), $"%{q}%") ||
                    EF.Functions.ILike(x.Name, $"%{q}%")
                );
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
        public async Task<ActionResult<EquipmentDTO>> PutEquipment(int id, EquipmentDTO dto)
        {
            var equipment = await _context.Equipment.FirstOrDefaultAsync(x => x.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            if (HttpContext.IsAdmin())
            {
                equipment.ShopId = dto.ShopId;
            }

            equipment.Name = dto.Name;
            equipment.EquipmentTypeId = dto.EquipmentTypeId;

            await _context.SaveChangesAsync();

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentDTO>> PostEquipment(EquipmentDTO dto)
        {
            var equipment = new Equipment
            {
                ShopId = HttpContext.IsAdmin() ? dto.ShopId : HttpContext.GetShopId(),
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

            equipment.Archived = true;
            await _context.SaveChangesAsync();

            return equipment;
        }
    }
}
