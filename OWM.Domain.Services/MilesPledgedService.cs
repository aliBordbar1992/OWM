using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class MilesPledgedService : Service<MilesPledged>, IMilesPledgedService
    {
        public MilesPledgedService(ITrackableRepository<MilesPledged> repository) : base(repository)
        {
        }
    }
}
