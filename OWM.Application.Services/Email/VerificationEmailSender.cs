using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace OWM.Application.Services.Email
{
    public class VerifyEmailEmailSender : EmailSender
    {
        private readonly string _name;
        public VerifyEmailEmailSender(IHostingEnvironment hostingEnvironment, string name, string emailAddress, string verificationAddress)
        : base(hostingEnvironment, new VerificationEmailTemplateBuilder(name, verificationAddress), emailAddress)
        {
            _name = name;
            Config();
        }

        protected sealed override void Config()
        {
            Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("OneWorldMarathon", EmailSenderConfigs.SendingEmailAddress));
            Message.To.Add(new MailboxAddress(_name, EmailAddress));
            Message.Subject = "Email verification";

            Message.Body = GetBody("verification-template.html").ToMessageBody();
        }
    }
}