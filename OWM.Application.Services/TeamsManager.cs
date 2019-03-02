using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWM.Application.Services.Exceptions;
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
        public event EventHandler<Exception> CreationFailed;
        public event EventHandler<MilesPledgedArgs> MilesPledged;
        public event EventHandler<MilesPledgedArgs> PledgedMilesUpdated;
        public event EventHandler<Exception> FailedToPledgeMiles;

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

                InsertMemberAsCreator(t, teamDto.ProfileId);
                InsertOccupations(teamDto, t);

                _teamService.ApplyChanges(t);
                await _unitOfWork.SaveChangesAsync();

                OnTeamCreated(new TeamCreatedArgs(t));
            }
            catch (Exception e)
            {
                OnCreationFaild(new TeamCreationFailedException("There was an error creating the team. Try again.", e, teamDto));
            }
        }

        private void InsertMemberAsCreator(Team t, int profileId)
        {
            t.Members.Add(new TeamMember
            {
                Team = t,
                ProfileId = profileId,
                IsCreator = true,
                KickedOut = false,
                TrackingState = TrackingState.Added
            });
        }
        private void InsertOccupations(CreateTeamDto teamDto, Team t)
        {
            if (!teamDto.OccupationFilter) return;

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
                OnFailedToPledgeMiles(e);
            }
        }

        public async Task<List<MyTeamsListDto>> GetListOfTeams(int profileId)
        {
            var teams = await _teamService.Queryable()
                .Where(x => x.Members.Any(m => m.ProfileId == profileId))
                .ToListAsync();
            
            var result = new List<MyTeamsListDto>();

            foreach (var team in teams)
            {
                var totalMilesCompleted = _milesPledgedService.Queryable().Where(x => x.Team.Id == team.Id)
                    .Select(x => x.CompletedMiles.Select(c => c.Miles).Sum()).Sum();

                var totalMilesPledged =
                    _milesPledgedService.Queryable().Where(x => x.Team.Id == team.Id).Sum(x => x.Miles);

                result.Add(new MyTeamsListDto
                {
                    TeamName = team.Name,
                    TeamId = team.Id,
                    TotalMilesCompleted = totalMilesCompleted,
                    TotalMilesPledged = totalMilesPledged,
                    MyCompletedMiles = team.PledgedMiles.Single(x => x.Profile.Id == profileId).CompletedMiles.Sum(x => x.Miles),
                    MyPledgedMiles = team.PledgedMiles.Single(x => x.Profile.Id == profileId).Miles,
                    IsCreator = team.Members.Any(x => x.IsCreator && x.ProfileId == profileId),
                    IsKickedOut = team.Members.Any(x => x.KickedOut && x.ProfileId == profileId)
                });
            }

            return result;
        }

        public void IncreaseMilesCompletedBy(int pledgedMileId, int profileId, float miles)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CompletedMiles> CompletedMiles(Profile profile, Team team = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> CloseTeam(int teamId, bool isOpen)
        {
            try
            {
                var team = await _teamService.FindAsync(teamId);
                team.IsClosed = !isOpen;
                await _unitOfWork.SaveChangesAsync();
                return 0;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        protected virtual void OnMilesPledged(MilesPledgedArgs e) => MilesPledged?.Invoke(this, e);
        protected virtual void OnPledgedMileUpdated(MilesPledgedArgs e) => PledgedMilesUpdated?.Invoke(this, e);
        protected virtual void OnFailedToPledgeMiles(Exception e) => FailedToPledgeMiles?.Invoke(this, e);

        protected virtual void OnTeamCreated(TeamCreatedArgs e) => TeamCreated?.Invoke(this, e);
        protected virtual void OnCreationFaild(Exception e) => CreationFailed?.Invoke(this, e);
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