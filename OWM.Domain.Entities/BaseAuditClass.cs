using System;
using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public abstract class BaseAuditClass : Entity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
