using RentApi.Api.Admin.Dashboards.Charts.Line;
using RentApi.Api.Admin.Dashboards.Charts.Pie;

namespace RentApi.Api.Admin.Dashboards
{
    public class DashboardsDTO
    {
        public PieData[] RentsPerEmployee { get; set; }
        public PieData[] RentsPerEquipmentType { get; set; }
        public LineData[] RentsPerShop { get; set; }
    }
}
