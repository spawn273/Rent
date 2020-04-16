namespace RentApi.Infrastructure.Database.Models
{
    public class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PricePerHour { get; set; }
        public int PricePerDay { get; set; }
    }
}
