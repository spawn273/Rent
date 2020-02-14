using RentApi.Database.Models;
using System;
using System.Linq;

namespace RentApi.Database
{
    public static class SeedHelper
    {
        public static void AutoSeed(RentApiDbContext context)
        {
            context.Database.EnsureDeleted();

            if (context.Database.EnsureCreated())
            {
                Seed(context);
            }
        }

        private static void Seed(RentApiDbContext context)
        {
            // Shops

            var shops = new Shop[]
            {
                new Shop
                {
                    Name = "Гидра"
                },
                new Shop
                {
                    Name = "Рамп"
                }
            };

            context.Shop.AddRange(shops);

            // Employees

            var employees = new Employee[]
            {
                new Employee
                {
                    Name = "Вован",
                    Shop = shops[0]
                },
                new Employee
                {
                    Name = "Димон",
                    Shop = shops[1]
                },
            };

            context.Employee.AddRange(employees);

            // Equipments

            var equipment = new Equipment[]
            {
                new Equipment
                {
                    Shop = shops[0],
                    Name = "руки збс",
                    EquipmentType = new EquipmentType
                    {
                        Name = "руки",
                        Price = 100
                    }
                },
                new Equipment
                {
                    Shop = shops[1],
                    Name = "ноги збс",
                    EquipmentType = new EquipmentType
                    {
                        Name = "ноги",
                        Price = 500
                    }
                }
            };

            context.Equipment.AddRange(equipment);

            context.SaveChanges();
        }
    }
}
