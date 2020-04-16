using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApi.Api.DTO
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ShopId { get; set; }
        public int EquipmentTypeId { get; set; }

        public int PricePerHour { get; set; }
        public int PricePerDay { get; set; }
    }
}
