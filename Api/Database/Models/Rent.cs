using System;

namespace RentApi.Database.Models
{
    public class Rent
    {
        public int Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
