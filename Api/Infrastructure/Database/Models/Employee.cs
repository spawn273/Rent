namespace RentApi.Infrastructure.Database.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //public string accountId { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
