using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;

namespace RentApi.Infrastructure.Database.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int ShopId { get; set; }

        public User User { get; set; }
        public Shop Shop { get; set; }
    }
}
