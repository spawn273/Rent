using RentApi.Infrastructure.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApi.Api.DTO
{
    public class RentDTO
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public int ShopId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime? Closed { get; set; }
        public int? Payment { get; set; }
        public string Comment { get; set; }

        public ICollection<int> EquipmentIds { get; set; }
    }
}
