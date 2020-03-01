using Microsoft.AspNetCore.Http;

namespace RentApi.Infrastructure
{
    public class ApiHelper
    {
        private HttpContext _context;

        public ApiHelper(IHttpContextAccessor accessor)
        {
            _context = accessor.HttpContext;
        }

        public void SetTotalCount(int count)
        {
            _context.Response.Headers.Add("X-Total-Count", count.ToString());
        }
    }
}
