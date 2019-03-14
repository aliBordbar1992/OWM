using System.Collections.Generic;

namespace OWM.Domain.Entities
{
    public class MessageBoard : BaseAuditClass
    {
        public MessageBoard()
        {
            Messages= new List<Message>();
            Participants = new List<Participant>();
        }

        public virtual Team ForTeam { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}