using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class CountryService : Service<Country>, ICountryService
    {
        public CountryService(ITrackableRepository<Country> repository) : base(repository)
        {
        }
    }
}