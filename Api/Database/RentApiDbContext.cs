using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApi.Database.Models;

namespace RentApi.Database
{
    public class RentApiDbContext : DbContext
    {
        public RentApiDbContext (DbContextOptions<RentApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentType> EquipmentType { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<RentApi.Database.Models.Employee> Employee { get; set; }
    }
}
