namespace SmartAnalytics.BASF.Backend.Application.Authorization.DTO
{
    public class PasswordResetRequest
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
