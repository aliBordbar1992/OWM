using System;

namespace OWM.Domain.Entities
{
    public class TeamInvitation : BaseAuditClass
    {
        public int SenderProfileId { get; set; }
        public int? RecipientProfileId { get; set; }
        public Guid InvitationGuid { get; set; }
        public Guid TeamGuid { get; set; }
        public string RecipientEmailAddress { get; set; }
        public bool Read { get; set; }
    }
}