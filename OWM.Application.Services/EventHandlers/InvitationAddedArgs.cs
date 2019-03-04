using System;
using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.EventHandlers
{
    public class AddInvitationArgs : EventArgs
    {
        public InvitationInformationDto Info { get; }

        public AddInvitationArgs(InvitationInformationDto info)
        {
            Info = info;
        }
    }
}