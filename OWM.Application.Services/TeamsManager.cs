using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class TeamsManagerService : ITeamsManagerService
    {
        private readonly ITeamService _teamService;
        private readonly IMilesPledgedService _milesPledgedService;
        private readonly IProfileService _profileService;
        private readonly IUnitOfWork _unitOfWork;

        public TeamsManagerService(ITeamService teamService, IMilesPledgedService milesPledgedService, IProfileService profileService, IUnitOfWork unitOfWork)
        {
            _teamService = teamService;
            _milesPledgedService = milesPledgedService;
            _profileService = profileService;
            _unitOfWork = unitOfWork;
        }

        public event EventHandler<TeamCreatedArgs> TeamCreated;
        public event EventHandler<TeamCreationFailedArgs> CreationFaild;
        public event EventHandler<MilesPledgedArgs> MilesPledged;
        public event EventHandler<MilesPledgedArgs> PledgedMilesUpdated;
        public event EventHandler<FailedToPledgeMilesdArgs> FailedToPledgeMiles;

        public async Task CreateTeam(CreateTeamDto teamDto)
        {
            try
            {
                Team t = new Team
                {
                    Name = teamDto.Name,
                    ShortDescription = teamDto.Description,
                    OccupationFilter = teamDto.OccupationFilter,
                    AgeRange = teamDto.Range,
                    Identity = Guid.NewGuid(),
                    IsClosed = false,
                    TrackingState = TrackingState.Added
                };

                if (teamDto.OccupationFilter)
                {
                    foreach (var ocpId in teamDto.OccupationIds)
                    {
                        t.AllowedOccupations.Add(new TeamOccupations
                        {
                            Team = t,
                            OccupationId = ocpId,
                            TrackingState = TrackingState.Added
                        });
                    }
                }
                _teamService.ApplyChanges(t);
                await _unitOfWork.SaveChangesAsync();

                OnTeamCreated(new TeamCreatedArgs(t));
            }
            catch (Exception e)
            {
                OnCreationFaild(new TeamCreationFailedArgs(e));
            }
        }

        public async Task PledgeMiles(int teamId, int profileId, float miles)
        {
            try
            {
                var profile = await _profileService.FindAsync(profileId);
                var team = await _teamService.FindAsync(teamId);
                MilesPledged mp = new MilesPledged
                {
                    Miles = miles,
                    Profile = profile,
                    Team = team
                };

                _milesPledgedService.Insert(mp);
                await _unitOfWork.SaveChangesAsync();

                OnMilesPledged(new MilesPledgedArgs(mp));
            }
            catch (Exception e)
            {
                if (await TeamJustCreated(teamId))
                {
                    await _teamService.DeleteAsync(teamId);
                    await _unitOfWork.SaveChangesAsync();
                }

                OnFailedToPledgeMiles(new FailedToPledgeMilesdArgs(e));
            }
        }

        private async Task<bool> TeamJustCreated(int teamId)
        {
            if (await _teamService.ExistsAsync(teamId))
            {
                var team = await _teamService.FindAsync(teamId);
                return team.PledgedMiles.Any();
            }

            return false;
        }

        public async Task IncreasePledgedMilesBy(int pledgedMileId, float miles)
        {
            try
            {
                var mp = await _milesPledgedService.Queryable().FirstAsync(x => x.Id == pledgedMileId);

                mp.Miles += miles;
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                OnFailedToPledgeMiles(new FailedToPledgeMilesdArgs(e));
            }
        }

        public void IncreaseMilesCompletedBy(int pledgedMileId, int profileId, float miles)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CompletedMiles> CompletedMiles(Profile profile, Team team = null)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void OnMilesPledged(MilesPledgedArgs e) => MilesPledged?.Invoke(this, e);
        protected virtual void OnPledgedMileUpdated(MilesPledgedArgs e) => PledgedMilesUpdated?.Invoke(this, e);
        protected virtual void OnFailedToPledgeMiles(FailedToPledgeMilesdArgs e) => FailedToPledgeMiles?.Invoke(this, e);

        protected virtual void OnTeamCreated(TeamCreatedArgs e) => TeamCreated?.Invoke(this, e);
        protected virtual void OnCreationFaild(TeamCreationFailedArgs e) => CreationFaild?.Invoke(this, e);
    }

    public class MilesPledgedArgs : EventArgs
    {
        public MilesPledged MilesPledged { get; }

        public MilesPledgedArgs(MilesPledged miles)
        {
            MilesPledged = miles;
        }
    }
    public class FailedToPledgeMilesdArgs : EventArgs
    {
        public Exception Exception { get; }

        public FailedToPledgeMilesdArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    public class TeamCreatedArgs : EventArgs
    {
        public Team Team { get; }

        public TeamCreatedArgs(Team team)
        {
            Team = team;
        }
    }
    public class TeamCreationFailedArgs : EventArgs
    {
        public Exception Exception { get; }

        public TeamCreationFailedArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}