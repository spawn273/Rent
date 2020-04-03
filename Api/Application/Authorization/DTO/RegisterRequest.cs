namespace SmartAnalytics.BASF.Backend.Application.Authorization.DTO
{
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //public ValidationResult Validate(UserFieldsValidator validator)
        //{
        //    return validator.Validate(new UserFields
        //    {
        //        Name = Name,
        //        Surname = Surname,
        //        Department = Department,
        //        Email = Email,
        //        Password = Password,
        //    }, ruleSet: "default,Email,Password");
        //}
    }
}
