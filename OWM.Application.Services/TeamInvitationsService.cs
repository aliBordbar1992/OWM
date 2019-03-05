using System;
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
        private readonly IUnitOfWork _unitOfWork;

        public TeamInvitationsService(ITeamInvitationService invitationService
            , ITeamService teamService
            , IUnitOfWork unitOfWork)
        {
            _invitationService = invitationService;
            _teamService = teamService;
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
                var team = _teamService.Queryable()
                    .First(x => x.Identity == dto.TeamGuid);

                bool exists = _invitationService.Queryable()
                    .AnyAsync(x => x.InvitationGuid == dto.InvitationGuid
                                   && x.TeamGuid == dto.TeamGuid
                                   && x.SenderProfileId == dto.SenderId).Result;

                if (exists) key = dto;
                else key = null;

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