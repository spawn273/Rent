using RentApi.Infrastructure.Database.Models;
using System.Linq;

namespace RentApi.Api.Guest
{
    public static class IQueryableExtensions
    {
        public static IQueryable<EquipmentDTO> ToDTO(this IQueryable<Equipment> rents)
        {
            return rents.Select(x => new EquipmentDTO
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
