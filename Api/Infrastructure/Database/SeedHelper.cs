using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApi.Infrastructure.Database
{
    public static class SeedHelper
    {
        public static void AutoSeed(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    context.Database.EnsureDeleted();

                    if (context.Database.EnsureCreated())
                    {
                        var userManager = services.GetRequiredService<UserManager<User>>();
                        var roleManager = services.GetRequiredService<RoleManager<Role>>();
                        SeedAsync(context, userManager, roleManager).Wait();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }

        private static async Task SeedAsync(AppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            // Users

            var users = new User[]
            {
                new User
                {
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    LastName = "Иванов",
                    UserName = "vova228@mail.ru",
                    Email = "vova228@mail.ru",
                    EmailConfirmed = true
                },
                new User
                {
                    FirstName = "Дмитрий",
                    MiddleName = "Дмитриевич",
                    LastName = "Дмитриев",
                    UserName = "dima420@mail.ru",
                    Email = "dima420@mail.ru",
                    EmailConfirmed = true
                }
            };

            await userManager.CreateAsync(users[0], "asdASD123!");
            await userManager.CreateAsync(users[1], "ZXCasd123!");

            // Shops

            var shops = new Shop[]
            {
                new Shop
                {
                    Name = "Прокат на Ленина"
                },
                new Shop
                {
                    Name = "Прокат на Мира"
                }
            };

            context.Shop.AddRange(shops);

            // Employees

            var employees = new Employee[]
            {
                new Employee
                {
                    Shop = shops[0],
                    User = users[0]
                },
                new Employee
                {
                    Shop = shops[1],
                    User = users[1]
                },
            };

            context.Employee.AddRange(employees);

            // EquipmentTypes
            var equipmentTypes = new EquipmentType[]
            {
                new EquipmentType
                {
                    Name = "Гидрокостюм",
                    PricePerHour = 100,
                    PricePerDay = 1000,
                },
                new EquipmentType
                {
                    Name = "Палатка 4-местная",
                    PricePerHour = 80,
                    PricePerDay = 800,
                },
                new EquipmentType
                {
                    Name = "Палатка 3-местная",
                    PricePerHour = 50,
                    PricePerDay = 550,
                },
                new EquipmentType
                {
                    Name = "Палатка 2-местная",
                    PricePerHour = 25,
                    PricePerDay = 250,
                },
                new EquipmentType
                {
                    Name = "Байдарка",
                    PricePerHour = 500,
                    PricePerDay = 1500
                },
                new EquipmentType
                {
                    Name = "Велосипед шоссейный",
                    PricePerHour = 200,
                    PricePerDay = 1200
                },
                new EquipmentType
                {
                    Name = "Велосипед горный",
                    PricePerHour = 300,
                    PricePerDay = 1300
                },
                new EquipmentType
                {
                    Name = "Велосипед городской",
                    PricePerHour = 500,
                    PricePerDay = 1500
                }
            };

            // Equipment

            var equipment = new Equipment[]
            {
                new Equipment
                {
                    Shop = shops[0],
                    Name = "Гидрокостюм короткий мужской Joss, 2,5 мм",
                    EquipmentType = equipmentTypes[0]
                },
                new Equipment
                {
                    Shop = shops[0],
                    Name = "Палатка 4-местная Outventure Trenton 4",
                    EquipmentType = equipmentTypes[1]
                },
               new Equipment
                {
                    Shop = shops[0],
                    Name = "Палатка 3-местная Outventure Tahoe Camo 3",
                    EquipmentType = equipmentTypes[2]
                },
               new Equipment
                {
                    Shop = shops[0],
                    Name = "Палатка 2-местная Outventure DOME 2",
                    EquipmentType = equipmentTypes[3]
                },

               new Equipment
                {
                    Shop = shops[1],
                    Name = "Байдарка Тритон Вуокса Н-2",
                    EquipmentType = equipmentTypes[4]
                },
               new Equipment
                {
                    Shop = shops[1],
                    Name = "Велосипед шоссейный CUBE Attain Race",
                    EquipmentType = equipmentTypes[5]
                },
               new Equipment
                {
                    Shop = shops[1],
                    Name = "Велосипед горный Stern Dynamic 2.0 26",
                    EquipmentType = equipmentTypes[6]
                },
               new Equipment
                {
                    Shop = shops[1],
                    Name = "Велосипед городской Stern Q-stom neon 28",
                    EquipmentType = equipmentTypes[7]
                },
               new Equipment
                {
                    Shop = shops[1],
                    Name = "Байдарка Тритон Нева-2",
                    EquipmentType = equipmentTypes[4]
                }
            };

            context.Equipment.AddRange(equipment);

            // Customers

            //var customers = new Customer[]
            //{
            //    new Customer
            //    {
            //        User = users[0]
            //    },
            //    new Customer
            //    {
            //        User = users[1]
            //    },
            //    new Customer
            //    {
            //        Name = "pizdanahui"
            //    }
            //};

            //context.Customer.AddRange(customers);

            // Rents

            var rents = new Rent[]
            {
                new Rent
                {
                    Shop = shops[0],
                    Employee = employees[0],
                    Customer = "Степанов Степан Степанович",
                    From = DateTime.Parse("2020-01-10"),
                    To = DateTime.Parse("2020-02-10"),
                    Closed = DateTime.Parse("2020-02-11"),
                    Payment = 5000,
                    Comment = "тел. 234-67-98"
                },
                new Rent
                {
                    Shop = shops[0],
                    Employee = employees[0],
                    Customer = "Павлов Павел Павлович",
                    From = DateTime.Parse("2019-01-10"),
                    To = DateTime.Parse("2021-02-10"),
                },
                new Rent
                {
                    Shop = shops[0],
                    Employee = employees[0],
                    Customer = "Егоров Егор Егорьевич",
                    From = DateTime.Parse("2019-01-10"),
                    To = DateTime.UtcNow
                },
                new Rent
                {
                    Shop = shops[1],
                    Employee = employees[1],
                    Customer = "Александров Александр Александрович",
                    From = DateTime.Parse("2020-01-11"),
                    To = DateTime.Parse("2020-02-12"),
                    RentEquipment = new List<RentEquipment>
                    {
                        new RentEquipment
                        {
                            Equipment = equipment[0]
                        }
                    }
                },
            };
            context.Rent.AddRange(rents);

            // RentEquipment

            var rentEquipment = new RentEquipment[]
            {
                new RentEquipment
                {
                    Rent = rents[0],
                    Equipment = equipment[0]
                },
                new RentEquipment
                {
                    Rent = rents[0],
                    Equipment = equipment[1]
                },
            };
            context.RentEquipment.AddRange(rentEquipment);

            await context.SaveChangesAsync();

            // Roles

            var roles = new Role[]
            {
                new Role { Name = "admin" },
                new Role { Name = "employee" }
            };
            await roleManager.CreateAsync(roles[0]);
            await roleManager.CreateAsync(roles[1]);

            await userManager.UpdateSecurityStampAsync(users[0]);
            await userManager.AddToRoleAsync(users[0], "employee");

            await userManager.UpdateSecurityStampAsync(users[1]);
            await userManager.AddToRoleAsync(users[1], "admin");
        }
    }
}
