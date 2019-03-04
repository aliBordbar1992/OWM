namespace OWM.Application.Services.Utils
{
    public static class Constants
    {
        public static string DateFormat => "yyyy/MM/dd";
        public static string DateFormat_LongMonth => "dd MMMM yyyy";

        public static string GetProfilePictures(string url)
        {
            return string.IsNullOrEmpty(url)
                ? "/img/img_Plaaceholder.jpg"
                : url;
        }
    }
}