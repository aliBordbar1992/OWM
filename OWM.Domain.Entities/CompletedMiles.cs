using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OWM.Domain.Entities
{
    public class CompletedMiles : BaseAuditClass
    {
        private MilesPledged _pledgedMile;
        public ILazyLoader LazyLoader { get; }

        public CompletedMiles()
        {
        }

        public CompletedMiles(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public float Miles { get; set; }

        public virtual MilesPledged PledgedMile
        {
            get => LazyLoader.Load(this, ref _pledgedMile);
            set => _pledgedMile = value;
        }
    }
}