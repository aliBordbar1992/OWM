using System.Collections.Generic;
using OWM.Application.Services.Dtos;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;

namespace OWM.Application.Services.Interfaces
{
    public interface ITeamsManager
    {
        void CreateTeam(CreateTeamDto teamDto);
        void PledgeMiles(int teamId, int profileId, int miles);
        void IncreaseMilesCompleted(int miles, int pledgedMileId, int profileId);
        IEnumerable<CompletedMiles> CompletedMiles(Profile profile, Team team = null);
    }
}