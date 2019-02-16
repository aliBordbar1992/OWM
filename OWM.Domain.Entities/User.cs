using System;
using OWM.Domain.Entities.Enums;

namespace OWM.Domain.Entities
{
    public class User : BaseAuditClass
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Occupation Occupation { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public GenderEnum Gender { get; set; }
        public UserIdentity Identity { get; set; }
    }
}
