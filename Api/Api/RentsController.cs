﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Api.Extensions;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure;
using System.Linq.Dynamic.Core;
using RentApi.Application;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : BaseApiController
    {
        private readonly AppDbContext _context;

        public RentsController(AppDbContext context)
        {
            _context = context;
        }

        public enum Order
        {
            ASC,
            DESC
        }
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(src, null);
        }

        // GET: api/Rents
        [HttpGet]
        public async Task<ActionResult<RentDTO[]>> GetRents(int? shopId,
            int _start = 0, int _end = 10, 
            string _sort = "id", string _order = "ASC",
            bool opened = false, DateTime? endLte = null
            )
        {
            var query = _context.Rent.AsQueryable();

            if (shopId.HasValue)
            {
                query = query.Where(x => x.ShopId == shopId);
            }

            if (opened)
            {
                query = query.Where(x => x.Closed == null);
            }

            if (endLte != null)
            {
                query = query.Where(x => x.To < endLte.Value.Ceil(TimeSpan.FromDays(1)).ToUniversalTime());
            }

            var count = await query.CountAsync();
            SetTotalCount(count);

            query = query.Skip(_start)
                .Take(_end - _start)
                .OrderBy($"{_sort} {_order}");

            var result = await query.ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("many")]
        public async Task<ActionResult<RentDTO[]>> GetMany([FromQuery] int[] id)
        {
            var result = await _context.Rent
                .Where(x => id.Contains(x.Id))
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        // GET: api/Rents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentDTO>> GetRent(int id)
        {
            var rent = await _context.Rent.Where(x => x.Id == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            if (rent == null)
            {
                return NotFound();
            }

            return rent;
        }

        // PUT: api/Rents/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ActionResult<RentDTO>> PutRent(int id, RentDTO rent)
        {
            var entity = await _context.Rent.Include(x => x.RentEquipment).FirstOrDefaultAsync(x => x.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            entity.Customer = rent.Customer;
            entity.From = rent.From;
            entity.To = rent.To;
            entity.Closed = rent.Closed;
            entity.Payment = rent.Payment;
            entity.Comment = rent.Comment;
            entity.RentEquipment = rent.EquipmentIds.Select(x => new RentEquipment { EquipmentId = x }).ToList();

            await _context.SaveChangesAsync();

            return rent;
        }

        [HttpPost]
        public async Task<ActionResult<RentDTO>> PostRent(RentDTO dto)
        {
            var rent = new Rent
            {
                EmployeeId = HttpContext.GetEmployeeId(),
                Customer = dto.Customer,
                ShopId = HttpContext.GetShopId(),
                From = dto.From,
                To = dto.To,
                Closed = null,
                Payment = dto.Payment,
                Comment = dto.Comment,
                RentEquipment = dto.EquipmentIds.Select(x => new RentEquipment { EquipmentId = x }).ToList()
            };
            _context.Rent.Add(rent);
            await _context.SaveChangesAsync();

            dto.Id = rent.Id;

            return CreatedAtAction("GetRent", new { id = rent.Id }, dto);
        }

        // DELETE: api/Rents/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rent>> DeleteRent(int id)
        {
            var rent = await _context.Rent.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }

            _context.Rent.Remove(rent);
            await _context.SaveChangesAsync();

            return rent;
        }
    }
}
