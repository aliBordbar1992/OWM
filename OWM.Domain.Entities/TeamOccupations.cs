using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OWM.Domain.Entities
{
    public class TeamOccupations : BaseAuditClass
    {
        private Team _team;
        private Occupation _occupation;
        public ILazyLoader LazyLoader { get; set; }

        public TeamOccupations()
        {
        }

        public TeamOccupations(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int TeamId { get; set; }

        public Team Team
        {
            get => LazyLoader.Load(this, ref _team);
            set => _team = value;
        }

        public int OccupationId { get; set; }

        public Occupation Occupation
        {
            get => LazyLoader.Load(this, ref _occupation);
            set => _occupation = value;
        }
    }
}