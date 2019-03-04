namespace OWM.Application.Services.Email
{
    public class TemplateHelper
    {
        public static string GetVerifyLink(string guid) => $"/verify/{guid}";

        public static string UsernameHolder = "<UserName/>";
        public static string UserFullNameHolder = "<UserFullName/>";
        public static string UserFirstNameHolder = "<UserFirstName/>";
        public static string UserSurNameHolder = "<UserSurName/>";
        public static string SenderNameHolder = "<SenderName/>";
        public static string RecipientNameHolder = "<RecipientName/>";

        public static string MessageHeaderHolder = "<MessageHeader/>";
        public static string MessageBodyHolder = "<MessageBody/>";
        public static string MessageFooterHolder = "<MessageFooter/>";

        public static string MessagePara_1_Holder = "<MessageP1/>";
        public static string MessagePara_2_Holder = "<MessageP2/>";
        public static string MessagePara_3_Holder = "<MessageP3/>";
        public static string MessagePara_4_Holder = "<MessageP4/>";
        public static string MessagePara_5_Holder = "<MessageP5/>";

        public static string MessageLink_1_Holder = "<MessageLink1/>";
        public static string MessageLink_2_Holder = "<MessageLink2/>";
        public static string MessageLink_3_Holder = "<MessageLink3/>";
        public static string MessageLink_4_Holder = "<MessageLink4/>";
        public static string MessageLink_5_Holder = "<MessageLink5/>";


    }
}