using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class OccupationService : Service<Occupation>, IOccupationService
    {
        public OccupationService(ITrackableRepository<Occupation> repository) : base(repository)
        {
        }
    }
}