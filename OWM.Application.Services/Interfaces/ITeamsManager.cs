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
        event EventHandler<TeamCreatedArgs> JoinedTeamSuccessfully;
        event EventHandler<Exception> JoinTeamFailed;

        Task CreateTeam(CreateTeamDto dto);
        Task JoinTeam(int teamId, int profileId);
        Task<int> CloseTeam(int teamId, bool closed);
        Task<int> UpdateDescription(int teamId, string description);
        Task<int> KickMember(int teamId, int profileId, int memberProfileId);
        Task<int> UnKickMember(int teamId, int profileId, int memberProfileId);


        int GetTeamId(Guid teamGuid);
        Task<MyTeamsListDto> GetMyTeam(int teamId, int profileId);
        Task<List<MyTeamsListDto>> GetListOfMyTeams(int profileId);
        Task<TeamInformationDto> GetTeamInformation(int teamId, bool getKickedMembers);
        Task<TeamInformationDto> GetTeamInformation(Guid teamGuid, bool getKickedMembers);
        Task<ProfileInformationDto> GetTeamMemberProfileInformation(int profileId);
        Task<TeamInvitationInformationDto> GetTeamInviteInformation(int teamId);
        Task<TeamInvitationInformationDto> GetTeamInviteInformation(Guid teamGuid);
        
        
        Task<bool> IsMemberOfTeam(int teamId, int profileId);
        Task<CanJoinTeamDto> CanJoinTeam(int teamId, int profileId);
        Task<bool> IsCreatorOfTeam(int teamId, int profileId);
        Task<bool> TeamExists(int teamId);
        Task<bool> TeamExists(Guid teamGuid);
        Task<bool> IsBlockedMember(int teamId, int profileId);
    }
}