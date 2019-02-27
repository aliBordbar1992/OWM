using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;

namespace OWM.Application.Services
{
    public class UserInformationService : IUserInformationService
    {
        private readonly IProfileService _userService;
        private Profile _user;

        public UserInformationService(IProfileService userService)
        {
            _userService = userService;
        }

        private void SetLocalUser(string identityId, bool includeRelations = false)
        {
            try
            {
                if (includeRelations)
                    _user = _userService.Queryable()
                        .Include(x => x.City.Country)
                        .Include(x => x.Ethnicity)
                        .Include(x => x.Occupation)
                        .Include(x => x.Identity)
                        .Include(x => x.Interests)
                        .Single(x => x.Identity.Id == identityId);
                else
                    _user = _userService.Queryable().Single(x => x.Identity.Id == identityId);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException($"No user found for {identityId}");
            }
        }
        private void SetLocalUser(int userId, bool includeRelations = false)
        {
            try
            {
                if (includeRelations)
                    _user = _userService.Queryable()
                        .Include(x => x.City.Country)
                        .Include(x => x.Ethnicity)
                        .Include(x => x.Occupation)
                        .Include(x => x.Identity)
                        .Include(x => x.Interests)
                        .Single(x => x.Id == userId);
                else
                    _user = _userService.FindAsync(userId).Result;
            }
            catch (Exception)
            {
                throw new ArgumentNullException($"No user found for id {userId}");
            }
        }

        public UserInformationDto GetUserProfile(string identityId)
        {
            SetLocalUser(identityId, true);
            return new UserInformationDto
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
                UserImage = string.IsNullOrEmpty(_user.ProfileImageUrl) ? "/img/img_Plaaceholder.jpg" : _user.ProfileImageUrl,
            };
        }
        public string GetUserFirstName(string identityId)
        {
            SetLocalUser(identityId);
            return _user.Name;
        }
        public string GetUserSurname(string identityId)
        {
            SetLocalUser(identityId);
            return _user.Surname;
        }
        public City GetUserCity(string identityId)
        {
            SetLocalUser(identityId);
            return _user.City;
        }
        public DateTime GetUserDateOfBirth(string identityId)
        {
            SetLocalUser(identityId);
            return _user.DateOfBirth;
        }
        public string GetUserOccupation(string identityId)
        {
            SetLocalUser(identityId);
            return _user.Occupation.Name;
        }
        public string GetUserEthnicity(string identityId)
        {
            SetLocalUser(identityId);
            return _user.Ethnicity.Name;
        }
        public List<Interest> GetUserInterests(string identityId)
        {
            SetLocalUser(identityId);
            return _user.Interests.ToList();
        }

        public UserInformationDto GetUserProfile(int userId)
        {
            SetLocalUser(userId, true);
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
        }
        public string GetUserFirstName(int userId)
        {
            SetLocalUser(userId);
            return _user.Name;
        }
        public string GetUserSurnam(int userId)
        {
            SetLocalUser(userId);
            return _user.Surname;
        }
        public City GetUserCity(int userId)
        {
            SetLocalUser(userId);
            return _user.City;
        }
        public DateTime GetUserDateOfBirth(int userId)
        {
            SetLocalUser(userId);
            return _user.DateOfBirth;
        }
        public string GetUserOccupation(int userId)
        {
            SetLocalUser(userId);
            return _user.Occupation.Name;
        }
        public string GetUserEthnicity(int userId)
        {
            SetLocalUser(userId);
            return _user.Ethnicity.Name;
        }
        public List<Interest> GetUserInterests(int userId)
        {
            SetLocalUser(userId);
            return _user.Interests.ToList();
        }
    }
}