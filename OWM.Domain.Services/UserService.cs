using OWM.Domain.Entities;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class UserService : Service<User>, IUserService
    {
        public UserService(ITrackableRepository<User> repository) : base(repository)
        {
        }
    }
}