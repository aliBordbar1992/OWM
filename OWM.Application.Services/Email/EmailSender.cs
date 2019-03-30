﻿using System.IO;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace OWM.Application.Services.Email
{
    public abstract class EmailSender : IEmailSender
    {
        protected readonly IHostingEnvironment HostingEnvironment;
        protected readonly ITemplateBuilder TemplateBuilder;
        protected readonly string EmailAddress;
        protected MimeMessage Message;

        protected EmailSender(IHostingEnvironment hostingEnvironment, ITemplateBuilder templateBuilder, string emailAddress)
        {
            HostingEnvironment = hostingEnvironment;
            TemplateBuilder = templateBuilder;
            EmailAddress = emailAddress;
        }

        protected abstract void Config();
        public void Send()
        {
            using (var client = new SmtpClient())
            {
                Message.Priority = MessagePriority.Normal;
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(EmailSenderConfigs.Host, EmailSenderConfigs.Port, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(EmailSenderConfigs.AuthenticateUsername, EmailSenderConfigs.AuthenticatePassword);

                client.Send(Message);
                client.Disconnect(true);
            }
        }
        protected BodyBuilder GetBody(string templateFile)
        {
            var builder = new BodyBuilder();
            string path = $"{HostingEnvironment.WebRootPath}/emailTemplate/{templateFile}";
            using (StreamReader sourceReader = System.IO.File.OpenText(path))
            {
                builder.HtmlBody = TemplateBuilder.Build(sourceReader.ReadToEnd());
            }

            return builder;
        }
    }
}