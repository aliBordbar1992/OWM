using System.ComponentModel.DataAnnotations;

namespace OWM.Domain.Entities
{
    public class Participant : BaseAuditClass
    {
        public Profile Profile{ get; set; }
        public MessageBoard Board { get; set; }

        [Timestamp]
        public byte[] LastReadTimeStamp { get; set; }
    }
}