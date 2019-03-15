using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OWM.Domain.Entities
{
    public class TeamMember : BaseAuditClass
    {
        private Team _team;
        private Profile _memberProfile;
        public ILazyLoader LazyLoader { get; }

        public TeamMember()
        {
        }

        public TeamMember(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int TeamId { get; set; }

        public Team Team
        {
            get => LazyLoader.Load(this, ref _team);
            set => _team = value;
        }

        public int ProfileId { get; set; }

        public Profile MemberProfile
        {
            get => LazyLoader.Load(this, ref _memberProfile);
            set => _memberProfile = value;
        }

        public bool IsCreator { get; set; }
        public bool KickedOut { get; set; }
    }
}