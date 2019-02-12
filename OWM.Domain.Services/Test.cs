using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class TestEntityService : Service<TestEntity>, ITestEntityService
    {
        public TestEntityService(ITrackableRepository<TestEntity> repository) : base(repository)
        {
        }
    }
}
