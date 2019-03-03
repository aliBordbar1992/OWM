using System.Collections.Generic;
using System.Threading.Tasks;
using OWM.Domain.Entities;
using URF.Core.Abstractions.Services;

namespace OWM.Domain.Services.Interfaces
{
    public interface ITeamService : IService<Team>
    {
        Task<IEnumerable<TeamMember>> GetTeamMembers(int teamId);
        Task<IEnumerable<TeamMember>> GetUnKickedTeamMembersAsync(int teamId);
        Task<IEnumerable<MilesPledged>> GetTeamMilesPledged(int teamId, bool removeKickedMembers = true);
        IEnumerable<CompletedMiles> GetTeamMilesCompleted(int teamId, bool removeKickedMembers = true);
        Task<float> GetMemberCompletedMiles(int teamId, int memberId);
        Task<float> GetMemberPledgedMiles(int teamId, int memberId);
    }
}