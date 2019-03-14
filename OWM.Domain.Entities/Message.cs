using System.ComponentModel.DataAnnotations;

namespace OWM.Domain.Entities
{
    public class Message : BaseAuditClass
    {
        public virtual Message ReplyToMessage { get; set; }
        public virtual MessageBoard Board { get; set; }
        public virtual Participant From { get; set; }

        public string Text { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}