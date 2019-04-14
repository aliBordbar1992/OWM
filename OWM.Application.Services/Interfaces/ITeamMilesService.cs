using System;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.EventHandlers;

namespace OWM.Application.Services.Interfaces
{
    public interface ITeamMilesService
    {
        event EventHandler<MilesPledgedArgs> MilesPledged;
        event EventHandler<MilesPledgedArgs> PledgedMilesUpdated;
        event EventHandler<Exception> FailedToPledgeMiles;

        Task PledgeMiles(PledgeMilesDto dto);
        Task EditPledgedMiles(int teamId, int profileId, float miles);
        Task IncreaseMilesCompletedBy(int teamId, int profileId, float miles);


        Task<TeamMilesInformationDto> GetTeamMilesInformation(int teamId, int profileId);
        string[] GetRecentMilePledges(int take);

        Task<CanEditMilesDto> CanEditMiles(int teamId, int profileId, float miles);
        //Task<bool> CanPledgeMiles(int teamId, int profileId);
        Task<bool> CanCompleteMiles(int teamId, int profileId);
    }
}