using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class CityService : Service<City>, ICityService
    {
        public CityService(ITrackableRepository<City> repository) : base(repository)
        {
        }
    }
}