using System;
using System.Collections.Generic;
using OWM.Domain.Entities.Enums;

namespace OWM.Domain.Entities
{
    public class User : BaseAuditClass
    {
        public User()
        {
            Interests = new List<Interest>();
        }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }
        public virtual Occupation Occupation { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public virtual UserIdentity Identity { get; set; }
        public virtual ICollection<Interest> Interests { get; set; }
    }
}
