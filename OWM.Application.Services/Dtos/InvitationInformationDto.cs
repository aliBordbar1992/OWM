namespace OWM.Application.Services.Dtos
{
    public class InvitationInformationDto
    {
        public string TeamGuid { get; set; }
        public string TeamName { get; set; }
        public string SenderFullName { get; set; }
        public string RecipientName { get; set; }
        public string EmailAddress { get; set; }
        public string Message { get; set; }
        public int TeamId { get; set; }
        public int SenderId { get; set; }
        public int? RecipientId { get; set; }
    }
}