using System;
using System.Threading.Tasks;
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
        private readonly IUnitOfWork _unitOfWork;

        public TeamInvitationsService(ITeamInvitationService invitationService, IUnitOfWork unitOfWork)
        {
            _invitationService = invitationService;
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
                    TeamId = dto.TeamId,
                    RecipientEmailAddress = dto.EmailAddress,
                    Read = false,
                    RecipientProfileId = dto.RecipientId
                };

                _invitationService.Insert(newInvitation);
                await _unitOfWork.SaveChangesAsync();

                OnInvitationAdded(new AddInvitationArgs(dto));
            }
            catch (Exception e)
            {
                OnInvitationAddFailed(new AddInvitationArgs(dto));
            }
        }

        protected virtual void OnInvitationAdded(AddInvitationArgs e) => InvitationAdded?.Invoke(this, e);
        protected virtual void OnInvitationAddFailed(AddInvitationArgs e) => InvitationAdded?.Invoke(this, e);
    }
}