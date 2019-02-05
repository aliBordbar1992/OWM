using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class Occupation : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}