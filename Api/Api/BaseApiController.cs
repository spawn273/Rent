using Microsoft.AspNetCore.Mvc;

namespace RentApi.Api
{
    public class BaseApiController : ControllerBase
    {
        protected void SetTotalCount(int count)
        {
            Response.Headers.Add("X-Total-Count", count.ToString());
        }
    }
}
