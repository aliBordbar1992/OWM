namespace OWM.Application.Services.Email
{
    public class ForgetPasswordTemplateBuilder : ITemplateBuilder
    {
        private string _msg1 = @"You received this email because there was a request to reset your password.";
        private string _msg2 = @"To reset your password, click on the link below";
        private string _link = "<a style=\"font-family: inherit;background: #2f833d;color:" +
                               "#f3f3f3;border: none;font-size: 20px;padding: 15px 20px;" +
                               "outline: none;cursor: pointer;text-decoration:none;margin-bottom:10px;\" href='vrLnk'>Reset password</a>";
        private readonly string _resetPasswordLink;

        public ForgetPasswordTemplateBuilder(string verificationLink)
        {
            _resetPasswordLink = verificationLink;
        }

        public string Build(string template)
        {
            template = template.Replace(TemplateHelper.MessagePara_1_Holder, _msg1);
            template = template.Replace(TemplateHelper.MessagePara_2_Holder, _msg2);
            template = template.Replace(TemplateHelper.MessageLink_1_Holder, _link.Replace("vrLnk", _resetPasswordLink));
            return template;
        }
    }
}