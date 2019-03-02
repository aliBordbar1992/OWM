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
        event EventHandler<Exception> CreationFailed;

        event EventHandler<MilesPledgedArgs> MilesPledged;
        event EventHandler<MilesPledgedArgs> PledgedMilesUpdated;
        event EventHandler<Exception> FailedToPledgeMiles;

        Task CreateTeam(CreateTeamDto dto);
        Task PledgeMiles(PledgeMilesDto dto);
        Task IncreasePledgedMilesBy(int pledgedMileId, float miles);
        Task<List<MyTeamsListDto>> GetListOfTeams(int profileId);
        void IncreaseMilesCompletedBy(int pledgedMileId, int profileId, float miles);
        IEnumerable<CompletedMiles> CompletedMiles(Profile profile, Team team = null);

        Task<int> CloseTeam(int teamId, bool closed);
    }
}