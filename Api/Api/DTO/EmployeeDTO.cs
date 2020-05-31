namespace RentApi.Api.DTO
{
    public class EmployeeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }
        public int? ShopId { get; set; }
    }
}
