using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SmartAnalytics.BASF.Backend.Infrastructure
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext context)
        {
            var id = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Convert.ToInt32(id);
        }

        public static int GetEmployeeId(this HttpContext context)
        {
            var id = context.User.FindFirstValue("employee");
            return Convert.ToInt32(id);
        }

        public static int GetShopId(this HttpContext context)
        {
            var id = context.User.FindFirstValue("shop");
            return Convert.ToInt32(id);
        }

        public static string GetRole(this HttpContext context)
        {
            return context.User.FindFirstValue(ClaimTypes.Role);
        }

        public static bool IsAdmin(this HttpContext context)
        {
            return context.GetRole() == "admin";
        }

        public static string GetBaseUrl(this HttpContext context)
        {
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}";
        }


        public static ListInfo GetListInfo(this HttpContext context) {
            return new ListInfo
            {
                Start = int.Parse(context.Request.Query["_start"].First()),
                End = int.Parse(context.Request.Query["_end"].First()),
                Descending = context.Request.Query["_order"].First() == "DESC",
                Sort = context.Request.Query["_sort"].First(),
            };
        }

    }

    public class ListInfo
    {
        public int Start { get; set; }
        public int End { get; set; }
        public bool Descending { get; set; }
        public string Sort { get; set; }
    }
}
