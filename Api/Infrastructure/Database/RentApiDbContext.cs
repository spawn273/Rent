using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApi.Infrastructure.Database.Models;

namespace RentApi.Infrastructure.Database
{
    public class RentApiDbContext : DbContext
    {
        public RentApiDbContext (DbContextOptions<RentApiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentType> EquipmentType { get; set; }
        public DbSet<Rent> Rent { get; set; }
        public DbSet<RentEquipment> RentEquipment { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // https://github.com/dotnet/efcore/issues/4711
            modelBuilder
               .Entity<Rent>()
               .Property(x => x.From)
               .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder
               .Entity<Rent>()
               .Property(x => x.To)
               .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
