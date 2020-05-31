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
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;

namespace RentApi.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : BaseApiController
    {
        private readonly AppDbContext _context;

        public ShopsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ShopDTO[]>> GetList(
            int _start = 0, int _end = 10,
            string _sort = "id", string _order = "ASC",
            string q = ""
            )
        {
            var query = _context.Shop.AsQueryable().Where(x => !x.Archived);

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => 
                    EF.Functions.ILike(x.Name, $"%{q}%") || 
                    EF.Functions.ILike(x.Address, $"%{q}%") ||
                    EF.Functions.ILike(x.Phone, $"%{q}%")
                );
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
        public async Task<ActionResult<ShopDTO[]>> GetMany([FromQuery] int[] id)
        {
            var result = await _context.Shop
                .Where(x => id.Contains(x.Id))
                .ToDTO()
                .ToArrayAsync();

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShopDTO>> Get(int id)
        {
            var shop = await _context.Shop.Where(x => x.Id == id)
                .ToDTO()
                .FirstOrDefaultAsync();

            if (shop == null)
            {
                return NotFound();
            }

            return shop;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ShopDTO>> Put(int id, ShopDTO dto)
        {
            var entity = await _context.Shop.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<ShopDTO>> Post(ShopDTO dto)
        {
            var shop = new Shop
            {
                Name = dto.Name,
                Address = dto.Address,
                Phone = dto.Phone
            };
            _context.Shop.Add(shop);
            await _context.SaveChangesAsync();

            dto.Id = shop.Id;

            return dto;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Shop>> Delete(int id)
        {
            var shop = await _context.Shop.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            shop.Archived = true;
            await _context.SaveChangesAsync();

            return shop;
        }
    }
}
