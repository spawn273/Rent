using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentApi.Infrastructure.Database;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;

namespace RentApi.Api.Admin.Dashboards
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DashboardsController : BaseApiController
    {
        private readonly AppDbContext _context;

        public DashboardsController(AppDbContext context)
        {
            _context = context;
        }

        public class DashboardsDTO
        {
            public PieData[] RentsPerMonthPie { get; set; }
            public PieData[] EquipmentTypes { get; set; }
            public LineData[] RentsPerShop { get; set; }
        }

        public class PieData
        {
            public string Id { get; set; }
            public int Value { get; set; }
        }


        public class LinePoint
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public class LineData
        {
            public string Id { get; set; }
            public IEnumerable<LinePoint> Data { get; set; }
        }

        [HttpGet("all")] 
        public async Task<DashboardsDTO> All()
        {
            var rentsPerMonthPie = await _context.Employee
                .Select(employee => new PieData
                {
                    Id = $"{employee.User.FirstName} {employee.User.MiddleName} {employee.User.LastName}",
                    Value = employee.Rents.Where(rent => rent.From > (DateTime.Now - TimeSpan.FromDays(360))).Count()
                })
                .Where(x => x.Value > 0)
                .ToArrayAsync();

            var equipmentTypes = await _context.EquipmentType
                .Select(type => new PieData
                {
                    Id = type.Name,
                    Value = _context.RentEquipment
                        .Where(req => req.Rent.From > (DateTime.Now - TimeSpan.FromDays(360)))
                        .Where(req => req.Equipment.EquipmentTypeId == type.Id)
                        .Count()
                })
                .Where(x => x.Value > 0)
                .ToArrayAsync();

            var shops = await _context.Shop.Include(x => x.Rents).ToListAsync();
            var rentsPerShop = shops
                .Select(shop => new LineData
                {
                    Id = shop.Name,
                    Data = shop.Rents.GroupBy(x => x.From.Month).Select(x => new LinePoint
                    {
                        X = x.Key,
                        Y = x.Count()
                    }).ToArray()
                })
                .ToArray();

            var aaa = rentsPerShop.Select(line =>
            {

                //var data = new LinePoint[12].Select((x, i) =>
                //{

                //    return new LinePoint
                //    {

                //    };
                //});

                var data = new LinePoint[12];
                for (int i = 0; i < 12; i++)
                {
                    //var y = line.Data.ElementAtOrDefault(i)?.Y ?? 0;
                    data[i] = new LinePoint
                    {
                        X = i + 1
                    };
                }

                foreach (var point in line.Data)
                {
                    data[point.X - 1].Y = point.Y;
                }
                return new LineData
                {
                    Id = line.Id,
                    Data = data
                };
            }).ToArray();

            return new DashboardsDTO
            {
                RentsPerMonthPie = rentsPerMonthPie,
                EquipmentTypes = equipmentTypes,
                RentsPerShop = aaa
            };
        }

    }
}
