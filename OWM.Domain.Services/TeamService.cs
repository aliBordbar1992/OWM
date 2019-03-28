using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class TeamService : Service<Team>, ITeamService
    {
        private readonly DbContext _context;
        private readonly ITrackableRepository<TeamMember> _teamMembers;
        private readonly ITrackableRepository<Profile> _profiles;


        public TeamService(ITrackableRepository<Team> repository, DbContext context) : base(repository)
        {
            _context = context;
            _teamMembers = new TrackableRepository<TeamMember>(context);
            _profiles = new TrackableRepository<Profile>(context);
        }

        public async Task<IEnumerable<TeamMember>> GetUnKickedTeamMembersAsync(int teamId)
        {
            var members = await _teamMembers.Query()
                .Where(x => x.TeamId == teamId && !x.KickedOut)
                .SelectAsync();

            return members.AsEnumerable();
        }

        public async Task<IEnumerable<TeamMember>> GetTeamMembers(int teamId)
        {
            var result = await _teamMembers.Query()
                .Where(x => x.TeamId == teamId)
                .SelectAsync();

            return result.AsEnumerable();
        }

        public async Task<IEnumerable<MilesPledged>> GetTeamMilesPledged(int teamId, bool removeKickedMembers = true)
        {
            var profiles = _context.Set<Profile>()
                .Where(x => x.Teams.Any(m => m.KickedOut != removeKickedMembers && m.TeamId == teamId))
                .AsEnumerable();

            var res = profiles.SelectMany(x => x.MilesPledged.Where(m => m.Team.Id == teamId).AsEnumerable());
            return res;
        }

        public IEnumerable<CompletedMiles> GetTeamMilesCompleted(int teamId, bool removeKickedMembers = true)
        {
            var profiles = _context.Set<Profile>()
                .Where(x => x.Teams.Any(m => m.KickedOut != removeKickedMembers && m.TeamId == teamId))
                .AsEnumerable();

            var pledgedMiles = profiles.SelectMany(x => x.MilesPledged.Where(m => m.Team.Id == teamId).AsEnumerable());
            return pledgedMiles.SelectMany(x => x.CompletedMiles.AsEnumerable());
        }

        public async Task<float> GetMemberCompletedMiles(int teamId, int memberId)
        {
            var team = await Repository.Query()
                .Where(x => x.Id == teamId)
                .SelectAsync();

            var pledgedMiles = team.SelectMany(x => x.PledgedMiles.Where(p => p.Profile.Id == memberId).AsEnumerable());

            var completedMiles = pledgedMiles.SelectMany(x => x.CompletedMiles.AsEnumerable());
            return completedMiles.Sum(x => x.Miles);
        }

        public async Task<float> GetMemberPledgedMiles(int teamId, int memberId)
        {
            var team = await Repository.Query()
                .Where(x => x.Id == teamId)
                .SelectAsync();

            var pledgedMiles = team.Select(x => x.PledgedMiles.Where(m => m.Profile.Id == memberId).AsEnumerable()).FirstOrDefault();
            return pledgedMiles?.Sum(x => x.Miles) ?? 0;
        }

        public int GetTotalMembers()
        {
            return _profiles.Queryable().Count();
        }
    }
}
