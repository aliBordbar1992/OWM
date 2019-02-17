using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using OWM.Application.Services.Email;
using OWM.Application.Services.Interfaces;

namespace OWM.Application.Services
{
    public class VerificationEmailSender
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITemplateBuilder _templateBuilder;
        private readonly string _name;
        private readonly string _emailAddress;
        private MimeMessage _message;
        public VerificationEmailSender(IHostingEnvironment hostingEnvironment, string name, string emailAddress, string verificationAddress)
        {
            _hostingEnvironment = hostingEnvironment;
            _templateBuilder = new VerificationEmailTemplateBuilder(name, verificationAddress);
            _name = name;
            _emailAddress = emailAddress;
            Config();
        }

        private void Config()
        {
            _message = new MimeMessage();
            _message.From.Add(new MailboxAddress("OneWorldMarathon", "owm.assistance@gmail.com"));
            _message.To.Add(new MailboxAddress(_name, _emailAddress));
            _message.Subject = "Email verification";

            _message.Body = GetBody().ToMessageBody();
        }

        public void Send()
        {
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("owm.assistance@gmail.com", "Owm@2019");

                client.Send(_message);
                client.Disconnect(true);
            }
        }

        private BodyBuilder GetBody()
        {
            var builder = new BodyBuilder();
            string path = $"{_hostingEnvironment.WebRootPath}/emailTemplate/verification-template.html";
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                builder.HtmlBody = _templateBuilder.Build(SourceReader.ReadToEnd());
            }

            return builder;
        }
    }
}