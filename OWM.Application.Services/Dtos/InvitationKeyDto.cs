using System;

namespace OWM.Application.Services.Dtos
{
    public class InvitationKeyDto
    {
        public int SenderId { get; set; }
        public string TeamIdentity { get; set; }
        public string InvitationIdentity { get; set; }

        public Guid TeamGuid => Guid.Parse(TeamIdentity);
        public Guid InvitationGuid => Guid.Parse(InvitationIdentity);
    }
}