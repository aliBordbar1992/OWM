using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class TestEntity : Entity
    {
        public int Id { get; set; }
        public string name { get; set; }
    }
}