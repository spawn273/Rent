using RentApi.Infrastructure.Database.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentApi.Api.Guest
{
    public static class IQueryableExtensions
    {
        public static IQueryable<EquipmentDTO> ToDTO(this IQueryable<Equipment> equipment)
        {
            return equipment.Select(x => new EquipmentDTO
            {
                Id = x.Id,
                Type = x.EquipmentType.Name,
                Name = x.Name,
                ShopId = x.ShopId,
                EquipmentTypeId = x.EquipmentTypeId,
                Available = !x.RentEquipment.Any()
            });
        }
    }
}
