using System.Collections.Generic;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;

namespace OWM.Application.Services
{
    public class TeamsManager : ITeamsManager
    {
        public void CreateTeam(CreateTeamDto teamDto)
        {
            throw new System.NotImplementedException();
        }

        public void PledgeMiles(int teamId, int profileId, int miles)
        {
            throw new System.NotImplementedException();
        }

        public void IncreaseMilesCompleted(int miles, int pledgedMileId, int profileId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CompletedMiles> CompletedMiles(Profile profile, Team team = null)
        {
            throw new System.NotImplementedException();
        }
    }
}