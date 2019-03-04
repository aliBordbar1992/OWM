namespace OWM.Domain.Entities
{
    public class TeamInvitation : BaseAuditClass
    {
        public int TeamId { get; set; }
        public int SenderProfileId { get; set; }
        public int? RecipientProfileId { get; set; }
        public string RecipientEmailAddress { get; set; }
        public bool Read { get; set; }
    }
}