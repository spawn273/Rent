using System.Collections.Generic;

namespace RentApi.Api.Admin.Dashboards.Charts.Line
{
    public class LineData
    {
        public string Id { get; set; }
        public IEnumerable<LinePoint> Data { get; set; }
    }
}
