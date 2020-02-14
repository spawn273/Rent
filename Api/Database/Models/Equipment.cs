namespace RentApi.Database.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ShopId { get; set; }
        public int EquipmentTypeId { get; set; }

        public Shop Shop { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
}
