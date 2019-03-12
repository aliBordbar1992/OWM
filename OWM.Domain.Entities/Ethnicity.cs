using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class Ethnicity : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}