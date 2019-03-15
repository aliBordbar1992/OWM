using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OWM.Domain.Entities
{
    public class MilesPledged : BaseAuditClass
    {
        private ILazyLoader LazyLoader { get; }
        private Profile _profile;
        private Team _team;
        private ICollection<CompletedMiles> _completedMiles;

        public MilesPledged()
        {
            CompletedMiles = new List<CompletedMiles>();
        }
        private MilesPledged(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }



        public float Miles { get; set; }
        public Profile Profile
        {
            get => LazyLoader.Load(this, ref _profile);
            set => _profile = value;
        }
        public Team Team
        {
            get => LazyLoader.Load(this, ref _team);
            set => _team = value;
        }
        public ICollection<CompletedMiles> CompletedMiles
        {
            get => LazyLoader.Load(this, ref _completedMiles);
            set => _completedMiles = value;
        }
    }
}