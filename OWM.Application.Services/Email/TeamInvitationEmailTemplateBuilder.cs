namespace OWM.Application.Services.Email
{
    public class TeamInvitationEmailTemplateBuilder : ITemplateBuilder
    {
        private string _link = "<a style=\"font-family: inherit;background: #2f833d;color:" +
                               "#f3f3f3;border: none;font-size: 20px;padding: 15px 20px;" +
                               "outline: none;cursor: pointer;text-decoration:none;margin-bottom:10px;\" href='vrLnk'>Join now</a>";

        private readonly string _senderName;
        private readonly string _recipientName;
        private readonly string _message;
        private readonly string _invitationLink;

        public TeamInvitationEmailTemplateBuilder(string senderName, string recipientName, string message, string invitationLink)
        {
            _senderName = senderName;
            _recipientName = recipientName;
            _message = message;
            _invitationLink = invitationLink;
        }

        public string Build(string template)
        {
            template = template.Replace(TemplateHelper.SenderNameHolder, _senderName);
            template = template.Replace(TemplateHelper.RecipientNameHolder, _recipientName);
            template = template.Replace(TemplateHelper.MessagePara_1_Holder, _message);
            template = template.Replace(TemplateHelper.MessageLink_1_Holder, _link.Replace("vrLnk", _invitationLink));
            return template;
        }
    }
}