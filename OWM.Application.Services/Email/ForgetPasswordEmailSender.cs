using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace OWM.Application.Services.Email
{
    public class ForgetPasswordEmailSender : EmailSender
    {
        public ForgetPasswordEmailSender(IHostingEnvironment hostingEnvironment, string emailAddress, string resestPasswordLink) :
            base(hostingEnvironment, new ForgetPasswordTemplateBuilder(resestPasswordLink), emailAddress)
        {
            Config();
        }

        protected sealed override void Config()
        {
            Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("OneWorldMarathon", "owm.assistance@gmail.com"));
            Message.To.Add(new MailboxAddress(EmailAddress, EmailAddress));
            Message.Subject = "Reset password";

            Message.Body = base.GetBody("forget-password-template.html").ToMessageBody();
        }
    }
}