﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Api.DTO;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : BaseApiController
    {
        private readonly RentApiDbContext _context;

        public RentsController(RentApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Rents
        [HttpGet]
        public async Task<ActionResult<object>> GetRent()
        {
            var result = await _context.Rent.ToArrayAsync();
            SetTotalCount(result.Length);
            return result;

            //SetTotalCount(1);
            //return new[]
            //{
            //    new
            //    {
            //        Id = 1,
            //        From = DateTime.Now,
            //        To = DateTime.Now,
            //        EquipmentIds = new int[] { 1, 2 }
            //    }
            //};
        }

        [HttpGet("many")]
        public async Task<ActionResult<Rent[]>> GetMany([FromQuery] int[] id)
        {
            var result = await _context.Rent
                .Where(x => id.Contains(x.Id))
                .ToArrayAsync();

            return result;
        }

        // GET: api/Rents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentDTO[]>> GetRent(int id)
        {
            //return new
            //{
            //    Id = 1,
            //    From = DateTime.Now,
            //    To = DateTime.Now,
            //    EquipmentIds = new int[] { 1, 2 }
            //};

            var rent = await _context.Rent.Where(x => x.Id == id)
                .Select(x => new RentDTO
                {
                    Id = x.Id,
                    From = x.From,
                    To = x.To,
                    EquipmentIds = x.RentEquipment.Select(x => x.Id).ToArray()
                })
                .ToArrayAsync();

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
        public async Task<IActionResult> PutRent(int id, RentDTO rent)
        {
            if (id != rent.Id)
            {
                return BadRequest();
            }

            _context.Entry(rent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
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

        // POST: api/Rents
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Rent>> PostRent(Rent rent)
        {
            _context.Rent.Add(rent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRent", new { id = rent.Id }, rent);
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

        private bool RentExists(int id)
        {
            return _context.Rent.Any(e => e.Id == id);
        }
    }
}