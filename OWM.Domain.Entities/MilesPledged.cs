using System.Collections.Generic;

namespace OWM.Domain.Entities
{
    public class MilesPledged : BaseAuditClass
    {
        public MilesPledged()
        {
            CompletedMiles = new List<CompletedMiles>();
        }
        public float Miles { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Team Team { get; set; }
        public ICollection<CompletedMiles> CompletedMiles { get; set; }
    }
}