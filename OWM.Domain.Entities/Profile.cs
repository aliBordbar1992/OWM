using System;
using System.Collections.Generic;
using OWM.Domain.Entities.Enums;

namespace OWM.Domain.Entities
{
    public class Profile : BaseAuditClass
    {
        public Profile()
        {
            Interests = new List<Interest>();
            MilesPledged = new List<MilesPledged>();
            Teams = new List<TeamMember>();
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }
        public virtual Occupation Occupation { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public virtual User Identity { get; set; }
        public ICollection<Interest> Interests { get; set; }
        public ICollection<MilesPledged> MilesPledged { get; set; }
        public ICollection<TeamMember> Teams { get; set; }
    }
}
