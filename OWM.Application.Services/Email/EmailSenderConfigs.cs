namespace OWM.Application.Services.Email
{
    public class EmailSenderConfigs
    {
        public static string SendingEmailAddress => "owm.assistance@gmail.com";

        public static string AuthenticateUsername => "owm.assistance@gmail.com";
        public static string AuthenticatePassword => "Owm@2019";

        public static string Host => "smtp.gmail.com";
        public static int Port => 587;
    }
}