using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartAnalytics.BASF.Backend.Infrastructure
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext context)
        {
            return 1;
            var id = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Convert.ToInt32(id);
        }

        public static int GetEmployeeId(this HttpContext context)
        {
            return 1;
            var id = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Convert.ToInt32(id);
        }

        public static int GetShopId(this HttpContext context)
        {
            return 1;
            var id = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Convert.ToInt32(id);
        }

        public static string GetBaseUrl(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
        }
    }
}
