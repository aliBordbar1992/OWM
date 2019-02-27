using System.Collections.Generic;
using OWM.Domain.Entities.Enums;

namespace OWM.Domain.Entities
{
    public class Team : BaseAuditClass
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public bool OccupationFilter { get; set; }
        public bool IsClosed { get; set; }
        public AgeRange AgeRange { get; set; }
        public ICollection<MilesPledged> PledgedMiles { get; set; }
    }
}