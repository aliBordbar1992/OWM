using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class City : Entity
    {
        public int Id { get; set; }
        public int CustomCityId { get; set; }
        public virtual Country Country { get; set; }
        public string Name { get; set; }
    }
}