using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.EventHandlers;
using OWM.Application.Services.Exceptions;
using OWM.Application.Services.Interfaces;
using OWM.Application.Services.Utils;
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
        #region Properties
        private readonly ITeamService _teamService;
        private readonly IMilesPledgedService _milesPledgedService;
        private readonly IProfileService _profileService;
        private readonly ICountryService _countryService;
        private readonly IUnitOfWork _unitOfWork;

        public event EventHandler<TeamCreatedArgs> TeamCreated;
        public event EventHandler<Exception> CreationFailed;

        public event EventHandler<TeamCreatedArgs> JoinedTeamSuccessfully;
        public event EventHandler<Exception> JoinTeamFailed;
        #endregion

        public TeamsManagerService(ITeamService teamService
            , IMilesPledgedService milesPledgedService
            , IProfileService profileService
            , IUnitOfWork unitOfWork, ICountryService countryService)
        {
            _teamService = teamService;
            _milesPledgedService = milesPledgedService;
            _profileService = profileService;
            _unitOfWork = unitOfWork;
            _countryService = countryService;
        }


        public async Task CreateTeam(CreateTeamDto teamDto)
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

            var creator = new TeamMember
            {
                Team = t,
                ProfileId = teamDto.ProfileId,
                IsCreator = true,
                KickedOut = false,
                TrackingState = TrackingState.Added
            };

            t.Members.Add(creator);

            InsertOccupations(teamDto, t);

            try
            {
                _teamService.ApplyChanges(t);
                await _unitOfWork.SaveChangesAsync();

                OnTeamCreated(new TeamCreatedArgs(t));
            }
            catch (Exception e)
            {
                //Rollback
                creator.TrackingState = TrackingState.Deleted;
                t.Members.Remove(creator);
                foreach (var occupation in t.AllowedOccupations)
                    occupation.TrackingState = TrackingState.Deleted;

                _teamService.ApplyChanges(t);
                await _unitOfWork.SaveChangesAsync();

                OnCreationFailed(new TeamCreationFailedException("There was an error creating the team. Try again.", e, teamDto));
            }
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


        public async Task JoinTeam(int teamId, int profileId)
        {
            var profile = await _profileService.Queryable()
                .FirstOrDefaultAsync(x => x.Id == profileId);

            if (profile == null)
                throw new UserNotFoundException<int>($"No user found for id {profileId}", new ArgumentNullException(),
                    profileId);

            var team = await _teamService.Queryable()
                .FirstOrDefaultAsync(x => x.Id == teamId);

            if (team == null) throw new TeamNotFoundException<int>($"No team found for id {teamId}", new ArgumentNullException(),
                teamId);

            var newMember = new TeamMember
            {
                Team = team,
                ProfileId = profile.Id,
                IsCreator = false,
                KickedOut = false,
                TrackingState = TrackingState.Added
            };
            try
            {
                team.Members.Add(newMember);

                _teamService.ApplyChanges(team);
                await _unitOfWork.SaveChangesAsync();

                OnJoinedTeamSuccess(new TeamCreatedArgs(team));
            }
            catch (Exception e)
            {
                team.Members.Remove(newMember);

                _teamService.ApplyChanges(team);
                await _unitOfWork.SaveChangesAsync();

                OnJoinedTeamFailed(e);
            }
        }

        private static void InsertMember(Team team, Profile profile)
        {
            
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

        public async Task<int> UpdateDescription(int teamId, string description)
        {
            try
            {
                var team = await _teamService.FindAsync(teamId);
                team.ShortDescription = description;
                await _unitOfWork.SaveChangesAsync();
                return 0;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public async Task<int> KickMember(int teamId, int profileId, int memberProfileId)
        {
            try
            {
                var team = await _teamService.Queryable()
                    .Include(x => x.Members)
                    .FirstOrDefaultAsync(x => x.Id == teamId && x.Members.Any(m => m.ProfileId == profileId));

                if (team == null) return -1;

                var teamMemberToKickOut =
                    team.Members.FirstOrDefault(x => x.ProfileId == memberProfileId && !x.KickedOut);
                if (teamMemberToKickOut == null) return -2;

                teamMemberToKickOut.KickedOut = true;
                await _unitOfWork.SaveChangesAsync();
                return 0;
            }
            catch (Exception e)
            {
                return -3;
            }
        }

        public async Task<int> UnKickMember(int teamId, int profileId, int memberProfileId)
        {
            try
            {
                var team = await _teamService.Queryable()
                    .Include(x => x.Members)
                    .FirstOrDefaultAsync(x => x.Id == teamId && x.Members.Any(m => m.ProfileId == profileId && m.IsCreator));

                if (team == null) return -1;

                var teamMemberToUnKickOut =
                    team.Members.FirstOrDefault(x => x.ProfileId == memberProfileId && x.KickedOut);
                if (teamMemberToUnKickOut == null) return -2;

                teamMemberToUnKickOut.KickedOut = false;
                await _unitOfWork.SaveChangesAsync();
                return 0;
            }
            catch (Exception e)
            {
                return -3;
            }
        }





        public int GetTeamId(Guid keyTeamGuid)
        {
            return _teamService.Queryable().Single(x => x.Identity == keyTeamGuid).Id;
        }

        public async Task<MyTeamsListDto> GetMyTeam(int teamId, int profileId)
        {
            var team = await _teamService.Queryable()
                .Include(x => x.Members)
                .Include(x => x.PledgedMiles)
                .ThenInclude(x => x.Profile)
                .Include(x => x.PledgedMiles)
                .ThenInclude(x => x.CompletedMiles)
                .SingleAsync(x => x.Id == teamId && x.Members.Any(m => m.ProfileId == profileId));

            var teamPledgedMiles = await _teamService.GetTeamMilesPledged(team.Id);
            var teamCompletedMiles = _teamService.GetTeamMilesCompleted(team.Id);

            var totalMilesCompleted = teamCompletedMiles.Sum(x => x.Miles);
            var totalMilesPledged = teamPledgedMiles.Sum(x => x.Miles);

            var t = team.PledgedMiles.Single(x => x.Profile.Id == profileId).CompletedMiles.Sum(x => x.Miles);

            MyTeamsListDto result = new MyTeamsListDto
            {
                TeamName = team.Name,
                TeamId = team.Id,
                TotalMilesCompleted = totalMilesCompleted,
                TotalMilesPledged = totalMilesPledged,
                MyCompletedMiles = team.PledgedMiles.Single(x => x.Profile.Id == profileId).CompletedMiles.Sum(x => x.Miles),
                MyPledgedMiles = team.PledgedMiles.Single(x => x.Profile.Id == profileId).Miles,
                IsCreator = team.Members.Any(x => x.IsCreator && x.ProfileId == profileId),
            };

            return result;
        }

        public async Task<List<MyTeamsListDto>> GetListOfMyTeams(int profileId)
        {
            var teams = await _teamService.Queryable()
                .Include(x => x.PledgedMiles)
                .ThenInclude(x => x.CompletedMiles)
                .Include(x => x.PledgedMiles)
                .ThenInclude(m => m.Profile)
                .Where(x => x.Members.Any(m => m.ProfileId == profileId))
                .ToListAsync();

            var result = new List<MyTeamsListDto>();

            foreach (var team in teams)
            {
                var teamPledgedMiles = await _teamService.GetTeamMilesPledged(team.Id);
                var teamCompletedMiles = _teamService.GetTeamMilesCompleted(team.Id);

                var totalMilesCompleted = teamCompletedMiles.Sum(x => x.Miles);
                var totalMilesPledged = teamPledgedMiles.Sum(x => x.Miles);

                result.Add(new MyTeamsListDto
                {
                    TeamName = team.Name,
                    TeamId = team.Id,
                    TotalMilesCompleted = totalMilesCompleted,
                    TotalMilesPledged = totalMilesPledged,
                    MyCompletedMiles = team.PledgedMiles.Single(x => x.Profile.Id == profileId).CompletedMiles.Sum(x => x.Miles),
                    MyPledgedMiles = team.PledgedMiles.Single(x => x.Profile.Id == profileId).Miles,
                    IsCreator = team.Members.Any(x => x.IsCreator && x.ProfileId == profileId),
                    IsBlocked = team.Members.Any(x => x.KickedOut && x.ProfileId == profileId),
                });
            }

            return result
                .OrderByDescending(x => x.IsCreator)
                .ThenByDescending(x => x.TotalMilesCompleted)
                .ThenByDescending(x => x.TotalMilesPledged)
                .ToList();
        }

        public async Task<TeamInformationDto> GetTeamInformation(int teamId, bool getKickedMembers)
        {
            var team = await _teamService.Queryable()
                .Include(x => x.AllowedOccupations)
                .ThenInclude(x => x.Occupation)
                .SingleAsync(x => x.Id == teamId);
            await _teamService.LoadRelatedEntities(team);

            var teamPledgedMiles = await _teamService.GetTeamMilesPledged(teamId);
            var teamCompletedMiles = _teamService.GetTeamMilesCompleted(teamId);

            var totalMilesCompleted = teamCompletedMiles.Sum(x => x.Miles);
            var totalMilesPledged = teamPledgedMiles.Sum(x => x.Miles);

            List<string> occupations = team.OccupationFilter
                ? team.AllowedOccupations.Select(o => o.Occupation.Name).ToList()
                : new List<string> { "All" };

            var result = new TeamInformationDto
            {
                TeamName = team.Name,
                DateCreated = team.Created,
                Description = team.ShortDescription,
                IsClosed = team.IsClosed,
                Occupations = occupations,
                TotalMilesCompleted = totalMilesCompleted,
                TotalMilesPledged = totalMilesPledged,
                TeamMembers = await GetTeamMembers(teamId, getKickedMembers)
            };

            return result;
        }
        public async Task<TeamInformationDto> GetTeamInformation(Guid teamGuid, bool getKickedMembers)
        {
            var team = await _teamService.Queryable()
                .Include(x => x.AllowedOccupations)
                .ThenInclude(x => x.Occupation)
                .SingleAsync(x => x.Identity == teamGuid);
            await _teamService.LoadRelatedEntities(team);

            var teamPledgedMiles = await _teamService.GetTeamMilesPledged(team.Id);
            var teamCompletedMiles = _teamService.GetTeamMilesCompleted(team.Id);

            var totalMilesCompleted = teamCompletedMiles.Sum(x => x.Miles);
            var totalMilesPledged = teamPledgedMiles.Sum(x => x.Miles);

            List<string> occupations = team.OccupationFilter
                ? team.AllowedOccupations.Select(o => o.Occupation.Name).ToList()
                : new List<string> { "All" };

            var result = new TeamInformationDto
            {
                TeamName = team.Name,
                DateCreated = team.Created,
                Description = team.ShortDescription,
                IsClosed = team.IsClosed,
                Occupations = occupations,
                TotalMilesCompleted = totalMilesCompleted,
                TotalMilesPledged = totalMilesPledged,
                TeamMembers = await GetTeamMembers(team.Id, getKickedMembers)
            };

            return result;
        }
        private async Task<List<TeamMemberInformationDto>> GetTeamMembers(int teamId, bool getKickedMembers)
        {
            var teamMembers = getKickedMembers
                ? await _teamService.GetTeamMembers(teamId)
                : await _teamService.GetUnKickedTeamMembersAsync(teamId);
            var teamMembersInformation = new List<TeamMemberInformationDto>();
            foreach (var member in teamMembers.ToList())
            {
                var memberProfile = member.MemberProfile;
                await _profileService.LoadRelatedEntities(memberProfile);
                teamMembersInformation.Add(new TeamMemberInformationDto
                {
                    TeamId = teamId,
                    ProfileId = memberProfile.Id,
                    FirstName = memberProfile.Name,
                    SurName = memberProfile.Surname,
                    City = memberProfile.City.Name,
                    Country = memberProfile.Country.Name,
                    ProfilePicture = Constants.GetProfilePictures(memberProfile.ProfileImageUrl),

                    MyCompletedMiles = await _teamService.GetMemberCompletedMiles(teamId, memberProfile.Id),
                    MyPledgedMiles = await _teamService.GetMemberPledgedMiles(teamId, memberProfile.Id),
                    IsCreator = member.IsCreator,
                    IsKickedOut = member.KickedOut,
                });
            }

            return teamMembersInformation
                .OrderByDescending(x => x.IsCreator)
                .ThenByDescending(x => x.MyCompletedMiles)
                .ThenByDescending(x => x.MyPledgedMiles)
                .ToList();
        }

        public async Task<ProfileInformationDto> GetTeamMemberProfileInformation(int profileId)
        {
            IUserInformationService uInfoSrvc = new UserInformationService(_profileService);
            var profileInfo = await uInfoSrvc.GetUserProfileInformationAsync(profileId);
            var myTeams = await GetListOfMyTeams(profileId);
            return new ProfileInformationDto
            {
                ProfilePicture = profileInfo.ProfilePicture,
                Country = profileInfo.CountryName,
                City = profileInfo.CityName,
                Surname = profileInfo.Surname,
                FirstName = profileInfo.Name,
                Occupation = profileInfo.Occupation,
                Interests = profileInfo.Interests.Select(x => x.Name).ToList(),
                MilesCompleted = profileInfo.MilesCompleted,
                MilesPledged = profileInfo.MilesPledged,
                TeamsCreated = myTeams.Where(x => x.IsCreator).Select(x => new TeamInfoDto()
                {
                    TeamName = x.TeamName,
                    Id = x.TeamId
                }).ToList(),
                TeamsJoined = myTeams.Where(x => !x.IsCreator).Select(x => new TeamInfoDto()
                {
                    TeamName = x.TeamName,
                    Id = x.TeamId
                }).ToList()
            };
        }

        public async Task<TeamInvitationInformationDto> GetTeamInviteInformation(int teamId)
        {
            var team = await _teamService.FindAsync(teamId);

            return new TeamInvitationInformationDto
            {
                TeamId = team.Id,
                TeamName = team.Name,
                TeamGuid = team.Identity.ToString()
            };
        }
        public async Task<TeamInvitationInformationDto> GetTeamInviteInformation(Guid teamGuid)
        {
            var team = await _teamService.Queryable()
                .SingleAsync(x => x.Identity == teamGuid);

            return new TeamInvitationInformationDto
            {
                TeamId = team.Id,
                TeamName = team.Name,
                TeamGuid = team.Identity.ToString()
            };
        }

        public TotalTeamInformationSummaryDto GetSummary()
        {
            return new TotalTeamInformationSummaryDto
            {
                MilesPledged = _milesPledgedService.GetTotalMilesPledged(),
                Countries = _countryService.Queryable().Count(),
                Participants = _teamService.GetTotalMembers()
            };
        }


        public async Task<bool> IsMemberOfTeam(int teamId, int profileId)
        {
            return await _teamService.Queryable()
                .AnyAsync(x => x.Id == teamId && x.Members.Any(m => m.ProfileId == profileId));
        }

        public async Task<CanJoinTeamDto> CanJoinTeam(int teamId, int profileId)
        {
            CanJoinTeamDto result = new CanJoinTeamDto();

            var team = await _teamService.Queryable()
                .Include(x => x.AllowedOccupations)
                .SingleAsync(x => x.Id == teamId);

            result.TeamIsClosed = team.IsClosed;
            result.IsAlreadyMember = await IsMemberOfTeam(teamId, profileId);
            result.OccupationFilter = team.OccupationFilter;

            IUserInformationService uInfoSrvc = new UserInformationService(_profileService);
            var profileOccupation = await uInfoSrvc.GetUserOccupationAsync(profileId);

            result.OccupationsMatch = team.AllowedOccupations.Any(x => x.OccupationId == profileOccupation.Id);

            var dateOfBirth = await uInfoSrvc.GetUserDateOfBirthAsync(profileId);
            var ageRange = AgeRangeCalculator.GetAgeRange(dateOfBirth);

            result.AgeRangeMatch = team.AgeRange == ageRange;

            //can join a team if:
            //1. team is not closed, and
            //2. user is not already a member of team, and
            //3. team does not have occupation filter, if it have, user occupation must match with team,
            //4. age ranges match
            result.FinalResult = (!result.TeamIsClosed) && !result.IsAlreadyMember &&
                                 (!result.OccupationFilter || (result.OccupationFilter && result.OccupationsMatch)) &&
                                 result.AgeRangeMatch;

            return result;
        }

        public async Task<bool> IsBlockedMember(int teamId, int profileId)
        {
            return await _teamService.Queryable()
                .AnyAsync(x => x.Id == teamId && x.Members.Any(m => m.ProfileId == profileId && m.KickedOut));
        }

        public async Task<bool> IsCreatorOfTeam(int teamId, int profileId)
        {
            return await _teamService.Queryable()
                .AnyAsync(x => x.Id == teamId && x.Members.Any(m => m.ProfileId == profileId && m.IsCreator));
        }

        public async Task<bool> TeamExists(int teamId)
        {
            return await _teamService.ExistsAsync(teamId);
        }

        public async Task<bool> TeamExists(Guid teamGuid)
        {
            return await _teamService.Queryable().AnyAsync(x => x.Identity == teamGuid);
        }



        #region Events
        protected virtual void OnTeamCreated(TeamCreatedArgs e) => TeamCreated?.Invoke(this, e);
        protected virtual void OnCreationFailed(Exception e) => CreationFailed?.Invoke(this, e);

        protected virtual void OnJoinedTeamSuccess(TeamCreatedArgs e) => JoinedTeamSuccessfully?.Invoke(this, e);
        protected virtual void OnJoinedTeamFailed(Exception e) => JoinTeamFailed?.Invoke(this, e);
        #endregion
    }
}