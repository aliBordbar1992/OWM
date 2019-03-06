using System;

namespace OWM.Application.Services.Dtos
{
    public class UserInvitationsDto
    {
        public int InvitationId { get; set; }

        public int TeamId { get; set; }
        public string TeamName { get; set; }

        public DateTime Created { get; set; }
        public bool Read { get; set; }

        public int SenderId { get; set; }
        public string SenderFullName { get; set; }
        public string SenderProfilePicture { get; set; }
    }
}