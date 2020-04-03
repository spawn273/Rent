using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartAnalytics.BASF.Backend.Application.Authorization
{
    public class AuthorizationOptions
    {
        public JwtOptions Jwt { get; set; }
    }

    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int Lifetime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}
