namespace OWM.UI.Web.Dtos
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string DisplayMessage { get; set; }
        public object Data { get; set; }
    }
}