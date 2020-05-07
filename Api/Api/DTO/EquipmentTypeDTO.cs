using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApi.Api.DTO
{
    public class EquipmentTypeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PricePerHour { get; set; }
        public int PricePerDay { get; set; }
    }
}
