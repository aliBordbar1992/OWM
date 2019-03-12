using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class Occupation : Entity
    {
        public Occupation()
        {
            InTeams = new List<TeamOccupations>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public ICollection<TeamOccupations> InTeams { get; set; }
    }
}