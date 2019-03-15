using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.EventHandlers;
using OWM.Application.Services.Exceptions;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class TeamMilesService : ITeamMilesService
    {
        private readonly ITeamsManagerService _teamsManager;
        private readonly ITeamService _teamService;
        private readonly IMilesPledgedService _milesPledgedService;
        private readonly IProfileService _profileService;
        private readonly IUnitOfWork _unitOfWork;

        public event EventHandler<MilesPledgedArgs> MilesPledged;
        public event EventHandler<MilesPledgedArgs> PledgedMilesUpdated;
        public event EventHandler<Exception> FailedToPledgeMiles;

        public TeamMilesService(ITeamsManagerService teamsManager
        , ITeamService teamService
        , IMilesPledgedService milesPledgedService
        , IProfileService profileService
        , IUnitOfWork unitOfWork)
        {
            _teamsManager = teamsManager;
            _teamService = teamService;
            _milesPledgedService = milesPledgedService;
            _profileService = profileService;
            _unitOfWork = unitOfWork;
        }

        public async Task PledgeMiles(PledgeMilesDto dto)
        {
            try
            {
                var profile = await _profileService.FindAsync(dto.ProfileId);
                var team = await _teamService.FindAsync(dto.TeamId);
                MilesPledged mp = new MilesPledged
                {
                    Miles = dto.Miles,
                    Profile = profile,
                    Team = team
                };

                _milesPledgedService.Insert(mp);
                await _unitOfWork.SaveChangesAsync();

                OnMilesPledged(new MilesPledgedArgs(mp));
            }
            catch (Exception e)
            {
                if (await TeamJustCreated(dto.TeamId))
                {
                    await _teamService.DeleteAsync(dto.TeamId);
                    await _unitOfWork.SaveChangesAsync();
                }

                OnFailedToPledgeMiles(new PledgedMilesFailedException("There was an error with your miles pledged. Try again.", e, dto));
            }
        }
        private async Task<bool> TeamJustCreated(int teamId)
        {
            if (await _teamService.ExistsAsync(teamId))
            {
                var team = await _teamService.FindAsync(teamId);
                return !team.PledgedMiles.Any();
            }

            return false;
        }

        public async Task IncreasePledgedMilesBy(int teamId, int profileId, float miles)
        {
            var mp = await _milesPledgedService.Queryable()
                //.Include(x => x.Team)
                //.Include(x => x.Profile)
                .FirstOrDefaultAsync(x => x.Team.Id == teamId
                                          && x.Profile.Id == profileId);

            if (mp == null) throw new ArgumentNullException("No miles pledged found for the given arguments");

            float originalMiles = mp.Miles;
            try
            {   
                mp.Miles += miles;
                await _unitOfWork.SaveChangesAsync();

                OnPledgedMileUpdated(new MilesPledgedArgs(mp));
            }
            catch (Exception e)
            {
                //Rollback
                mp.Miles = originalMiles;
                await _unitOfWork.SaveChangesAsync();

                OnFailedToPledgeMiles(e);
            }
        }

        public async Task IncreaseMilesCompletedBy(int teamId, int profileId, float miles)
        {
            var mp = await _milesPledgedService.Queryable()
                //.Include(x => x.Team)
                //.Include(x => x.Profile)
                //.Include(x => x.CompletedMiles)
                .FirstOrDefaultAsync(x => x.Team.Id == teamId
                                          && x.Profile.Id == profileId);

            if (mp == null) throw new ArgumentNullException("No miles pledged found for the given arguments");

            var pledgedMiles = mp.Miles;
            var completedMiles = mp.CompletedMiles.Sum(x => x.Miles);

            if (completedMiles + miles > pledgedMiles)
            {
                var failedException = new CompleteMilesFailedException(
                    $"Cannot complete miles more than {pledgedMiles - completedMiles}.",
                    new InvalidEnumArgumentException(), null);

                OnFailedToPledgeMiles(failedException);

                return;
            }

            CompletedMiles cm = new CompletedMiles
            {
                Miles = miles,
                PledgedMile = mp,
                TrackingState = TrackingState.Added
            };

            try
            {
                mp.CompletedMiles.Add(cm);
                _milesPledgedService.ApplyChanges(mp);

                await _unitOfWork.SaveChangesAsync();

                OnPledgedMileUpdated(new MilesPledgedArgs(mp));
            }
            catch (Exception e)
            {
                //Rollback
                cm.TrackingState = TrackingState.Deleted;
                mp.CompletedMiles.Remove(cm);
                _milesPledgedService.ApplyChanges(mp);
                await _unitOfWork.SaveChangesAsync();

                OnFailedToPledgeMiles(e);
            }
        }

        public async Task<TeamMilesInformationDto> GetTeamMilesInformation(int teamId, int profileId)
        {
            MyTeamsListDto myTeam = await _teamsManager.GetMyTeam(teamId, profileId);
            var teamMiles = new TeamMilesInformationDto
            {
                TeamName = myTeam.TeamName,

                MyCompletedMiles = myTeam.MyCompletedMiles,
                MyPledgedMiles = myTeam.MyPledgedMiles,
                MyCompletedMilesPercentage =
                    GetPercentage(myTeam.MyCompletedMiles, myTeam.MyPledgedMiles).ToString("0.0"),

                TeamCompletedMiles = myTeam.TotalMilesCompleted,
                TeamTotalMilesPledged = myTeam.TotalMilesPledged,
                TeamCompletedMilesPercentage =
                    GetPercentage(myTeam.TotalMilesCompleted, myTeam.TotalMilesPledged).ToString("0.0"),

                CanPledgeMiles = await CanPledgeMiles(teamId, profileId),
                CanCompleteMiles = await CanCompleteMiles(teamId, profileId),
            };

            return teamMiles;
        }

        public string[] GetRecentMilePledges(int take)
        {
            var ttt = _milesPledgedService.GetRecentMilePledges(10).ToList();

            var t = _milesPledgedService.GetRecentMilePledges(take)
                .Select(x => new
                {
                    x.Profile.Name,
                    CityName = x.Profile.City.Name,
                    CountryName = x.Profile.Country.Name,
                    x.Miles
                }).ToList();

            return t.Select(x => $"{x.Name} from {x.CityName}, {x.CountryName} just pledged {x.Miles} miles")
                .ToArray();
        }

        public async Task<bool> CanPledgeMiles(int teamId, int profileId)
        {
            //can pledge miles if:
            //1. total miles pledged in team is under 26.22 miles
            //2. member completed miles are greater than or equals to his/her pledged miles

            //1
            var teamPledgedMiles = await _teamService.GetTeamMilesPledged(teamId);
            bool notOver26Miles = teamPledgedMiles.Sum(x => x.Miles) <= 26.22;
            if (notOver26Miles) return true;

            //2
            var pledgedMiles = await _teamService.GetMemberPledgedMiles(teamId, profileId);
            var completedMiles = await _teamService.GetMemberCompletedMiles(teamId, profileId);
            var completed = completedMiles >= pledgedMiles;

            if (completed) return true;

            return false;
        }

        public async Task<bool> CanCompleteMiles(int teamId, int profileId)
        {
            //can pledge miles if:
            //1. total miles pledged in team is over 26.22 miles, and
            //2. member completed miles are lesser than his/her pledged miles

            //1
            var teamPledgedMiles = await _teamService.GetTeamMilesPledged(teamId);
            bool isOver26Miles = teamPledgedMiles.Sum(x => x.Miles) >= 26.22;

            //2
            var pledgedMiles = await _teamService.GetMemberPledgedMiles(teamId, profileId);
            var completedMiles = await _teamService.GetMemberCompletedMiles(teamId, profileId);
            var notCompleted = completedMiles < pledgedMiles;

            if (isOver26Miles && notCompleted) return true;

            return false;
        }

        private float GetPercentage(float a, float b)
        {
            return (a / b) * 100;
        }


        protected virtual void OnMilesPledged(MilesPledgedArgs e) => MilesPledged?.Invoke(this, e);
        protected virtual void OnPledgedMileUpdated(MilesPledgedArgs e) => PledgedMilesUpdated?.Invoke(this, e);
        protected virtual void OnFailedToPledgeMiles(Exception e) => FailedToPledgeMiles?.Invoke(this, e);
    }
}