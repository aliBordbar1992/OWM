using System;
using System.Collections.Generic;
using OWM.Domain.Entities.Enums;

namespace OWM.Domain.Entities
{
    public class Team : BaseAuditClass
    {
        public Team()
        {
            PledgedMiles = new List<MilesPledged>();
            AllowedOccupations = new List<TeamOccupations>();
            Members = new List<TeamMember>();
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public bool OccupationFilter { get; set; }
        public bool IsClosed { get; set; }
        public Guid Identity { get; set; }
        public AgeRange AgeRange { get; set; }
        public ICollection<MilesPledged> PledgedMiles { get; set; }
        public ICollection<TeamOccupations> AllowedOccupations { get; set; }
        public ICollection<TeamMember> Members { get; set; }
    }
}