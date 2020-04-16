using System;
using System.Collections.Generic;

namespace RentApi.Infrastructure.Database.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public int EmployeeId { get; set; }
        public int ShopId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Employee Employee { get; set; }
        public Shop Shop { get; set; }
        public List<RentEquipment> RentEquipment { get; set; }
    }
}
