using System;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.EventHandlers;

namespace OWM.Application.Services.Interfaces
{
    public interface ITeamInvitationsService
    {
        event EventHandler<AddInvitationArgs> InvitationAdded;
        event EventHandler<AddInvitationArgs> InvitationAddFailed;

        Task AddInvitation(InvitationInformationDto dto);
    }
}