using Microsoft.AspNetCore.Identity;

namespace SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
