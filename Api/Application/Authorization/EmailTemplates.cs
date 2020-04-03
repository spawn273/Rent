namespace SmartAnalytics.BASF.Backend.Application.Authorization
{
    public static class EmailTemplates
    {
        public static string EmailConfirmation(string name, string surname, string email, string link)
        {
            return $@"Dear {name} {surname},<br/>
<br/>
Thank you for registering at Basf.com. Your account is created and must be activated before you can use it. To activate the account click on the following link:<br/>
{link}<br/>
The link is valid for 24 hours.<br/>
<br/>
After activation you may login to https://agricultureterminal.basf.com/ using the following username:<br/>
<br/>
{email}<br/>
<br/>
Thank you,<br/>
Sincerely<br/>
BASF Agriculture Terminal Team
";
        }

        public static string PasswordResetLink(string name, string surname, string link)
        {
            return $@"Dear {name} {surname},<br/>
You wanted to recover your account password. Click this link to change your password:<br/>
{link}<br/>
The link is valid for 24 hours.<br/>
<br/>
Thank you,<br/>
Sincerely<br/>
BASF Agriculture Terminal Team
";
        }

        public static string PasswordResetEmailNotFound(string email, string link)
        {
            return $@"
We have received an account recovery request on Basf.com for e-mail: {email}, but email does not exist in our records. You can
<a href='{link}'> sign up here</a>.<br/>
Thank you,<br/>
Sincerely<br/>
BASF Agriculture Terminal Team
";
        }
    }
}
