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
                    FirstName = "vova",
                    UserName = "vova228@mail.ru",
                    Email = "vova228@mail.ru",
                    EmailConfirmed = true
                },
                new User
                {
                    FirstName = "dima",
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
                        PricePerHour = 100,
                        PricePerDay = 1000,
                    }
                },
                new Equipment
                {
                    Shop = shops[0],
                    Name = "локти збс",
                    EquipmentType = new EquipmentType
                    {
                        Name = "локти",
                        PricePerHour = 100,
                        PricePerDay = 1000,
                    }
                },
                new Equipment
                {
                    Shop = shops[1],
                    Name = "ноги збс",
                    EquipmentType = new EquipmentType
                    {
                        Name = "ноги",
                        PricePerHour = 500,
                        PricePerDay = 5000
                    }
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
                    Customer = "Жоран",
                    From = DateTime.Parse("2020-01-10"),
                    To = DateTime.Parse("2020-02-10"),
                    Closed = DateTime.Parse("2020-02-11"),
                    Payment = 5000
                },
                new Rent
                {
                    Shop = shops[0],
                    Employee = employees[0],
                    Customer = "Степан",
                    From = DateTime.Parse("2019-01-10"),
                    To = DateTime.Parse("2021-02-10"),
                },
                new Rent
                {
                    Shop = shops[1],
                    Employee = employees[1],
                    Customer = "Пашок",
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
