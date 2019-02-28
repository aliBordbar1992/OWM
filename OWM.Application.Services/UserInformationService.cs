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

namespace OWM.Application.Services
{
    public class UserInformationService : IUserInformationService
    {
        private readonly IProfileService _profileService;
        private Profile _user;

        public UserInformationService(IProfileService userService)
        {
            _profileService = userService;
        }

        private async Task SetLocalUser(string identityId, bool includeRelations = false)
        {
            try
            {
                if (includeRelations)
                    _user = await _profileService.Queryable()
                        .Include(x => x.City.Country)
                        .Include(x => x.Ethnicity)
                        .Include(x => x.Occupation)
                        .Include(x => x.Identity)
                        .Include(x => x.Interests)
                        .SingleAsync(x => x.Identity.Id == identityId);
                else
                    _user = await _profileService.Queryable().SingleAsync(x => x.Identity.Id == identityId);
            }
            catch (Exception e)
            {
                throw new UserNotFoundException($"No user found for {identityId}");
            }
        }
        private async Task SetLocalUser(int userId, bool includeRelations = false)
        {
            try
            {
                if (includeRelations)
                    _user = await _profileService.Queryable()
                        .Include(x => x.City.Country)
                        .Include(x => x.Ethnicity)
                        .Include(x => x.Occupation)
                        .Include(x => x.Identity)
                        .Include(x => x.Interests)
                        .SingleAsync(x => x.Id == userId);
                else
                    _user = await _profileService.FindAsync(userId);
            }
            catch (Exception)
            {
                throw new ArgumentNullException($"No user found for id {userId}");
            }
        }

        public async Task<UserInformationDto> GetUserProfileInformationAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId, true);
                var n = new UserInformationDto
                {
                    Ethnicity = _user.Ethnicity.Name,
                    Interest = string.Join(",", _user.Interests.Select(x => x.Name).ToList()),
                    Email = _user.Identity.Email,
                    Occupation = _user.Occupation.Name,
                    Name = _user.Name,
                    CityName = _user.City.Name,
                    Phone = _user.Identity.PhoneNumber,
                    Gender = (int)_user.Gender,
                    Birthday = _user.DateOfBirth.ToString(Utils.Constants.DateFormat),
                    CityId = _user.City.Id,
                    CountryName = _user.Country.Name,
                    OccupationId = _user.Occupation.Id,
                    Surname = _user.Surname,
                    EthnicityId = _user.Ethnicity.Id,
                    MilesCompleted = "MilesCompleted",
                    MilesPledged = "MilesPledged",
                    TeamJoined = "TeamJoined",
                    UserImage = string.IsNullOrEmpty(_user.ProfileImageUrl)
                        ? "/img/img_Plaaceholder.jpg"
                        : _user.ProfileImageUrl,
                };

                return n;
            });
        }
        public async Task<int> GetUserProfileIdAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId);
                return _user.Id;
            });
        }
        public async Task<string> GetUserFirstNameAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId);
                return _user.Name;
            });
        }
        public async Task<string> GetUserSurnameAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId);
                return _user.Surname;
            });
        }
        public async Task<City> GetUserCityAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId, true);
                return _user.City;
            });
        }
        public async Task<DateTime> GetUserDateOfBirthAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId);
                return _user.DateOfBirth;
            });
        }
        public async Task<Occupation> GetUserOccupationAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId, true);
                return _user.Occupation;
            });
        }
        public async Task<Ethnicity> GetUserEthnicityAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId, true);
                return _user.Ethnicity;
            });
        }
        public async Task<List<Interest>> GetUserInterestsAsync(string identityId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(identityId, true);
                return _user.Interests.ToList();
            });
        }

        public async Task<UserInformationDto> GetUserProfileInformationAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId, true);
                return new UserInformationDto
                {
                    Ethnicity = _user.Ethnicity.Name,
                    Interest = string.Join(",", _user.Interests.ToList()),
                    Email = _user.Identity.Email,
                    Occupation = _user.Occupation.Name,
                    Name = _user.Name,
                    CityName = _user.City.Name,
                    Phone = _user.Identity.PhoneNumber,
                    Gender = (int)_user.Gender,
                    Birthday = _user.DateOfBirth.ToString("MM/dd/yyyy"),
                    CityId = _user.City.Id,
                    CountryName = _user.Country.Name,
                    OccupationId = _user.Occupation.Id,
                    Surname = _user.Surname,
                    EthnicityId = _user.Ethnicity.Id,
                    MilesCompleted = "MilesCompleted",
                    MilesPledged = "MilesPledged",
                    TeamJoined = "TeamJoined",
                    UserImage = _user.ProfileImageUrl,
                };
            });
        }
        public async Task<int> GetUserProfileIdAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId);
                return _user.Id;
            });
        }
        public async Task<string> GetUserFirstNameAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId);
                return _user.Name;
            });
        }
        public async Task<string> GetUserSurnamAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId);
                return _user.Surname;
            });
        }
        public async Task<City> GetUserCityAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId, true);
                return _user.City;
            });
        }
        public async Task<DateTime> GetUserDateOfBirthAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId);
                return _user.DateOfBirth;
            });
        }
        public async Task<Occupation> GetUserOccupationAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId, true);
                return _user.Occupation;
            });
        }
        public async Task<Ethnicity> GetUserEthnicityAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId, true);
                return _user.Ethnicity;
            });
        }
        public async Task<List<Interest>> GetUserInterestsAsync(int userId)
        {
            return await Task.Run(async () =>
            {
                await SetLocalUser(userId, true);
                return _user.Interests.ToList();
            });
        }
    }
}