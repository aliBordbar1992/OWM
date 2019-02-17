namespace OWM.Application.Services.Email
{
    public class VerificationEmailTemplateBuilder : ITemplateBuilder
    {
        private string _fullName;
        private string _msg1 = @"Your registration has been completed.";
        private string _msg2 = @"To verify your account, click on the button below";
        private string _link = "<a style=\"font-family: inherit;background: #2f833d;color:" +
                               "#f3f3f3;border: none;font-size: 20px;padding: 15px 20px;" +
                               "outline: none;cursor: pointer;text-decoration:none;margin-bottom:10px;\" href='vrLnk'>Verify</a>";
        private string _verificationLink;

        public VerificationEmailTemplateBuilder(string fullName, string verificationLink)
        {
            _fullName = fullName;
            _verificationLink = verificationLink;
        }

        public string Build(string template)
        {
            template = template.Replace(TemplateHelper.UserFullNameHolder, _fullName);
            template = template.Replace(TemplateHelper.MessagePara_1_Holder, _msg1);
            template = template.Replace(TemplateHelper.MessagePara_2_Holder, _msg2);
            template = template.Replace(TemplateHelper.MessageLink_1_Holder, _link.Replace("vrLnk", _verificationLink));
            return template;
        }
    }
}