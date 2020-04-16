using SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities;

namespace RentApi.Infrastructure.Database.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
