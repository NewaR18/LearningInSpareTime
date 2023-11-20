using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityLearningSecondTry.EmailConfigurations
{
    public class EmailSender : IEmailSender
    {
        /*
         Made this mail manually because we used builder.Services.AddIdentity instead of AddDefaultIdentity
        */
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
