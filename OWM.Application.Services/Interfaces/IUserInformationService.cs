using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Interfaces
{
    public interface IUserInformationService
    {
        Task<UserInformationDto> GetUserProfileInformationAsync(string identityId);
        Task<int> GetUserProfileIdAsync(string identityId);
        Task<string> GetUserFirstNameAsync(string identityId);
        Task<string> GetUserSurnameAsync(string identityId);
        Task<City> GetUserCityAsync(string identityId);
        Task<DateTime> GetUserDateOfBirthAsync(string identityId);
        Task<Occupation> GetUserOccupationAsync(string identityId);
        Task<Ethnicity> GetUserEthnicityAsync(string identityId);
        Task<List<Interest>> GetUserInterestsAsync(string identityId);

        Task<UserInformationDto> GetUserProfileInformationAsync(int userId);
        Task<int> GetUserProfileIdAsync(int userId);
        Task<string> GetUserFirstNameAsync(int userId);
        Task<string> GetUserSurnamAsync(int userId);
        Task<City> GetUserCityAsync(int userId);
        Task<DateTime> GetUserDateOfBirthAsync(int userId);
        Task<Occupation> GetUserOccupationAsync(int userId);
        Task<Ethnicity> GetUserEthnicityAsync(int userId);
        Task<List<Interest>> GetUserInterestsAsync(int userId);
    }
}