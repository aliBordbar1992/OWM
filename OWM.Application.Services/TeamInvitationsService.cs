using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.EventHandlers;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class TeamInvitationsService : ITeamInvitationsService
    {
        private readonly ITeamInvitationService _invitationService;
        private readonly ITeamService _teamService;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamsManager;
        private readonly IProfileService _profileService;
        private readonly IUnitOfWork _unitOfWork;

        public TeamInvitationsService(ITeamInvitationService invitationService
            , ITeamService teamService
            , IUserInformationService userInformation
            , ITeamsManagerService teamsManager
            , IUnitOfWork unitOfWork)
        {
            _invitationService = invitationService;
            _teamService = teamService;
            _userInformation = userInformation;
            _teamsManager = teamsManager;
            _unitOfWork = unitOfWork;
        }

        public event EventHandler<AddInvitationArgs> InvitationAdded;
        public event EventHandler<AddInvitationArgs> InvitationAddFailed;
            
        public async Task AddInvitation(InvitationInformationDto dto)
        {
            try
            {
                var newInvitation = new TeamInvitation
                {
                    SenderProfileId = dto.SenderId,
                    TeamGuid = Guid.Parse(dto.TeamGuid),
                    RecipientEmailAddress = dto.EmailAddress,
                    Read = false,
                    RecipientProfileId = dto.RecipientId,
                    InvitationGuid = Guid.NewGuid()
                };

                _invitationService.Insert(newInvitation);
                await _unitOfWork.SaveChangesAsync();

                dto.InvitationGuid = newInvitation.InvitationGuid.ToString();
                dto.Token = GenerateKey(dto.SenderId, dto.TeamGuid, dto.InvitationGuid);
                OnInvitationAdded(new AddInvitationArgs(dto));
            }
            catch (Exception e)
            {
                OnInvitationAddFailed(new AddInvitationArgs(dto));
            }
        }

        private string GenerateKey(int senderId, string teamGuid, string invitationGuid)
        {
            return $"{senderId}@{teamGuid}:{invitationGuid}";
        }

        public InvitationKeyDto DecryptKey(string token)
        {
            string senderId = token.Substring(0, token.IndexOf('@'));
            string teamGuid = Slice(token, token.IndexOf('@') + 1, token.IndexOf(':'));
            string invitationGuid = token.Substring(token.IndexOf(':') + 1);

            return new InvitationKeyDto
            {
                TeamIdentity = teamGuid,
                InvitationIdentity = invitationGuid,
                SenderId = int.Parse(senderId)
            };
        }

        public bool TryVerifyToken(string token, out InvitationKeyDto key)
        {
            try
            {
                var dto = DecryptKey(token);
                bool exists = _invitationService.Queryable()
                    .AnyAsync(x => x.InvitationGuid == dto.InvitationGuid
                                   && x.TeamGuid == dto.TeamGuid
                                   && x.SenderProfileId == dto.SenderId).Result;

                key = exists ? dto : null;

                return exists;
            }
            catch (Exception e)
            {
                key = null;
                return false;
            }
        }

        public void UpdateInvitations(string email, int profileId)
        {
            var invitations = _invitationService.Queryable()
                .Where(x => x.RecipientEmailAddress == email)
                .ToList();

            foreach (var invitation in invitations)
            {
                invitation.RecipientProfileId = profileId;
            }

            _unitOfWork.SaveChangesAsync().RunSynchronously();
        }

        public bool HasInvitations(int profileId)
        {
            return _invitationService.Queryable()
                .Any(x => x.RecipientProfileId == profileId && !x.Read);
        }

        public async Task<List<UserInvitationsDto>> GetInvitations(int profileId)
        {
            var invitations = await _invitationService.Queryable()
                .Where(x => x.RecipientProfileId == profileId)
                .OrderBy(x => x.SenderProfileId)
                .ThenBy(x => x.TeamGuid)
                .ToListAsync();

            var sender = new UserInformationDto();
            var teamInvitation = new TeamInvitationInformationDto();

            var result = new List<UserInvitationsDto>();

            foreach (var invitation in invitations)
            {
                if (sender.ProfileId != invitation.SenderProfileId)
                    sender = await _userInformation.GetUserProfileInformationAsync(invitation.SenderProfileId);

                if (teamInvitation.TeamGuid != invitation.TeamGuid.ToString())
                    teamInvitation = await _teamsManager.GetTeamInviteInformation(invitation.TeamGuid);

                result.Add(new UserInvitationsDto
                {
                    InvitationId = invitation.Id,
                    TeamId = teamInvitation.TeamId,
                    TeamName = teamInvitation.TeamName,
                    Created = invitation.Created,
                    Read = invitation.Read,
                    SenderId = invitation.SenderProfileId,
                    SenderFullName = sender.Name + " " + sender.Surname,
                    SenderProfilePicture = sender.ProfilePicture
                });
            }

            return result.OrderByDescending(x => x.Created).ToList();
        }

        public async Task FlagAsRead(int invitationId)
        {
            var i = await _invitationService.FindAsync(invitationId);
            i.Read = true;

            await _unitOfWork.SaveChangesAsync();
        }

        private string Slice(string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end = source.Length + end;
            }
            int len = end - start;               // Calculate length
            return source.Substring(start, len); // Return Substring of length
        }

        protected virtual void OnInvitationAdded(AddInvitationArgs e) => InvitationAdded?.Invoke(this, e);
        protected virtual void OnInvitationAddFailed(AddInvitationArgs e) => InvitationAdded?.Invoke(this, e);
    }
}