using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;

namespace RentApi.Infrastructure.Database
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
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

            modelBuilder
               .Entity<Equipment>()
                .Property(x => x.Id)
                .HasIdentityOptions(10000);

            modelBuilder
               .Entity<Rent>()
                .Property(x => x.Id)
                .HasIdentityOptions(20000);

            // https://github.com/dotnet/efcore/issues/4711
            modelBuilder
               .Entity<Rent>()
               .Property(x => x.From)
               .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder
               .Entity<Rent>()
               .Property(x => x.To)
               .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder
               .Entity<Rent>()
               .Property(x => x.Closed)
               .HasConversion(v => v, v => DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));

        }
    }
}
