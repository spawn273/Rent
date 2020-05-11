namespace RentApi.Api.Guest
{
    public class EquipmentDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool Available { get; set; }

        public int ShopId { get; set; }
        public int EquipmentTypeId { get; set; }
    }
}
