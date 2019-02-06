using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class UserIdentityService : Service<UserIdentity>, IUserIdentityService
    {
        public UserIdentityService(ITrackableRepository<UserIdentity> repository) : base(repository)
        {
        }
    }
}