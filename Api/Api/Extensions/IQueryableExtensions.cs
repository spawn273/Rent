using RentApi.Api.DTO;
using RentApi.Infrastructure.Database.Models;
using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;
using System.Linq;

namespace RentApi.Api.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<RentDTO> ToDTO(this IQueryable<Rent> rents)
        {
            return rents.Select(x => new RentDTO
            {
                Id = x.Id,
                Customer = x.Customer,
                ShopId = x.ShopId,
                EmployeeId = x.EmployeeId,
                From = x.From,
                To = x.To,
                Closed = x.Closed,
                Payment = x.Payment,
                Comment = x.Comment,
                EquipmentIds = x.RentEquipment.Select(x => x.EquipmentId).ToArray()
            });
        }

        public static IQueryable<EquipmentDTO> ToDTO(this IQueryable<Equipment> rents)
        {
            return rents.Select(x => new EquipmentDTO
            {
                Id = x.Id,
                Name = x.Name,
                ShopId = x.ShopId,
                EquipmentTypeId = x.EquipmentTypeId,
                RentIds = x.RentEquipment.Where(x => x.Rent.Closed != null).Select(x => x.RentId).ToArray()
            });
        }

        public static IQueryable<EquipmentTypeDTO> ToDTO(this IQueryable<EquipmentType> types)
        {
            return types.Select(x => new EquipmentTypeDTO
            {
                Id = x.Id,
                Name = x.Name,
                PricePerDay = x.PricePerDay,
                PricePerHour = x.PricePerHour
            });
        }

        public static IQueryable<EmployeeDTO> ToDTO(this IQueryable<Employee> types)
        {
            return types.Select(x => new EmployeeDTO
            {
                Id = x.Id,
                Name = $"{x.User.FirstName} {x.User.MiddleName} {x.User.LastName}",
                ShopId = x.ShopId,
                UserId = x.UserId
            });
        }

        public static IQueryable<RoleDTO> ToDTO(this IQueryable<Role> roles)
        {
            return roles.Select(x => new RoleDTO
            {
                Id = x.Name,
                Info = x.Info
            });
        }
    }
}
