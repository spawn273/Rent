using System.Collections.Generic;

namespace RentApi.Infrastructure.Database.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public bool Archived { get; set; }

        public List<Rent> Rents { get; set; }
    }
}
