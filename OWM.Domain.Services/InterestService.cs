using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class InterestService : Service<Interest>, IInterestService
    {
        public InterestService(ITrackableRepository<Interest> repository) : base(repository)
        {
        }
    }
}
