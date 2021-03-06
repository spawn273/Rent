﻿using Microsoft.AspNetCore.Identity;
using RentApi.Infrastructure.Database.Models;

namespace SmartAnalytics.BASF.Backend.Infrastructure.Database.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool Deleted { get; set; }

        public Employee Employee { get; set; }
        public Role Role { get; set; }
    }
}
