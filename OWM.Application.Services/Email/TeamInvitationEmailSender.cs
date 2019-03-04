using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace OWM.Application.Services.Email
{
    public class TeamInvitationEmailSender : EmailSender
    {
        private readonly string _teamName;
        private readonly string _senderName;
        private readonly string _recipientName;
        public TeamInvitationEmailSender(IHostingEnvironment hostingEnvironment, string teamName, 
            string senderName, string recipientName, string emailAddress, string inviteMessage, string invitationAddress)
            : base(hostingEnvironment, new TeamInvitationEmailTemplateBuilder(senderName, recipientName, inviteMessage, invitationAddress), emailAddress)
        {
            _teamName = teamName;
            _senderName = senderName;
            _recipientName = recipientName;
            Config();
        }

        protected sealed override void Config()
        {
            Message = new MimeMessage();
            Message.From.Add(new MailboxAddress($"{_senderName} | Invitation", "owm.assistance@gmail.com"));
            Message.To.Add(new MailboxAddress(_recipientName, EmailAddress));
            Message.Subject = $"{_senderName} | Join {_teamName}";

            Message.Body = GetBody("team-invitation-template.html").ToMessageBody();
        }
    }
}