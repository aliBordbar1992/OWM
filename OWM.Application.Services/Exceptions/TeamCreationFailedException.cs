using System;
using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.Exceptions
{
    public class TeamCreationFailedException : SystemException
    {
        public TeamCreationFailedException(Exception e, CreateTeamDto data)
        {
        }
        public TeamCreationFailedException(string displayMessage, Exception e, CreateTeamDto data) : base(displayMessage)
        {
        }
    }
}