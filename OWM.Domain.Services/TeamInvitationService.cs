using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class TeamInvitationService : Service<TeamInvitation>, ITeamInvitationService
    {
        public TeamInvitationService(ITrackableRepository<TeamInvitation> repository) : base(repository)
        {
        }
    }
}
