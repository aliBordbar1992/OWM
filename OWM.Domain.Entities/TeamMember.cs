namespace OWM.Domain.Entities
{
    public class TeamMember : BaseAuditClass
    {
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int ProfileId { get; set; }
        public Profile MemberProfile { get; set; }

        public bool IsCreator { get; set; }
        public bool KickedOut { get; set; }
    }
}