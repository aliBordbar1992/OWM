using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class TeamOccupations : BaseAuditClass
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int OccupationId { get; set; }
        public Occupation Occupation { get; set; }
    }
}