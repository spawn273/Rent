namespace RentApi.Database.Models
{
    public class RentEquipment
    {
        public int Id { get; set; }
        public int RentId { get; set; }
        public int EquipmentId { get; set; }

        public Rent Rent { get; set; }
        public Equipment Equipment { get; set; }
    }
}
