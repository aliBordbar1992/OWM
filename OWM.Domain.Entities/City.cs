using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class City : Entity
    {
        public int Id { get; set; }
        public Country Country { get; set; }
        public string Name { get; set; }
    }
}