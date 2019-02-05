using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class EthnicityService : Service<Ethnicity>, IEthnicityService
    {
        public EthnicityService(ITrackableRepository<Ethnicity> repository) : base(repository)
        {
        }
    }
}