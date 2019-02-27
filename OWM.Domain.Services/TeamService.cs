using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class TeamService : Service<Team>, ITeamService
    {
        public TeamService(ITrackableRepository<Team> repository) : base(repository)
        {
        }
    }
}
