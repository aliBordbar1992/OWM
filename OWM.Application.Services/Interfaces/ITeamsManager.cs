using System;
using System.Collections.Generic;
using OWM.Application.Services.Dtos;
using OWM.Domain.Entities;
using System.Threading.Tasks;

namespace OWM.Application.Services.Interfaces
{
    public interface ITeamsManagerService
    {
        event EventHandler<TeamCreatedArgs> TeamCreated;
        event EventHandler<TeamCreationFailedArgs> CreationFaild;

        event EventHandler<MilesPledgedArgs> MilesPledged;
        event EventHandler<MilesPledgedArgs> PledgedMilesUpdated;
        event EventHandler<FailedToPledgeMilesdArgs> FailedToPledgeMiles;

        Task CreateTeam(CreateTeamDto teamDto);
        Task PledgeMiles(int teamId, int profileId, float miles);
        Task IncreasePledgedMilesBy(int pledgedMileId, float miles);
        void IncreaseMilesCompletedBy(int pledgedMileId, int profileId, float miles);
        IEnumerable<CompletedMiles> CompletedMiles(Profile profile, Team team = null);
    }
}