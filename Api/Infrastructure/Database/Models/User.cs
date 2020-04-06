using Microsoft.AspNetCore.Identity;
using RentApi.Infrastructure.Database.Models;

namespace SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public Employee Employee { get; set; }
    }
}
