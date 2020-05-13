namespace RentApi.Api.Admin
{
    public class AccountDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
        public string Name { get; set; }

        public int? ShopId { get; set; }
    }
}
