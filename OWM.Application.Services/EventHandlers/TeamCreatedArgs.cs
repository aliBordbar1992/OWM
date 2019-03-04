using System;
using OWM.Domain.Entities;

namespace OWM.Application.Services.EventHandlers
{
    public class TeamCreatedArgs : EventArgs
    {
        public Team Team { get; }

        public TeamCreatedArgs(Team team)
        {
            Team = team;
        }
    }
}