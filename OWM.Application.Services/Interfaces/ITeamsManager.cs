using System;
using System.Collections.Generic;
using OWM.Application.Services.Dtos;
using OWM.Domain.Entities;
using System.Threading.Tasks;
using OWM.Application.Services.EventHandlers;

namespace OWM.Application.Services.Interfaces
{
    public interface ITeamsManagerService
    {
        event EventHandler<TeamCreatedArgs> TeamCreated;
        event EventHandler<Exception> CreationFailed;

        event EventHandler<MilesPledgedArgs> MilesPledged;
        event EventHandler<MilesPledgedArgs> PledgedMilesUpdated;
        event EventHandler<Exception> FailedToPledgeMiles;

        Task CreateTeam(CreateTeamDto dto);
        Task PledgeMiles(PledgeMilesDto dto);
        Task IncreasePledgedMilesBy(int pledgedMileId, float miles);
        Task<List<MyTeamsListDto>> GetListOfMyTeams(int profileId);
        void IncreaseMilesCompletedBy(int pledgedMileId, int profileId, float miles);
        IEnumerable<CompletedMiles> CompletedMiles(Profile profile, Team team = null);

        Task<int> CloseTeam(int teamId, bool closed);
        Task<int> UpdateDescription(int teamId, string description);
        Task<int> KickMember(int teamId, int profileId, int memberProfileId);
        Task<int> UnKickMember(int teamId, int profileId, int memberProfileId);

        Task<TeamInformationDto> GetTeamInformation(int teamId, bool getKickedMembers = false);
        Task<TeamInformationDto> GetTeamInformation(Guid teamGuid, bool getKickedMembers);

        Task<bool> IsMemberOfTeam(int teamId, int userId); Task<ProfileInformationDto> GetTeamMemberProfileInformation(int profileId);
        Task<bool> CanJoinTeam(int teamId, int profileId);
        Task<bool> IsCreatorOfTeam(int teamId, int profileId);
        Task<TeamInvitationInformationDto> GetTeamInviteInformation(int teamId);

        int GetTeamId(Guid keyTeamGuid);
    }
}