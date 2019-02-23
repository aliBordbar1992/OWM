using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class UserService : Service<Profile>, IProfileService
    {
        public UserService(ITrackableRepository<Profile> repository) : base(repository)
        {
        }
    }
}