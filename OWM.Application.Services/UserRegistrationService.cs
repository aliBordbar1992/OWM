using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;
using OWM.Domain.Services.Interfaces;
using System;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserService _userServices;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IEthnicityService _ethnicityService;
        private readonly IOccupationService _occupationService;
        private readonly IUnitOfWork _unitOfWork;

        public event EventHandler<UserRegisteredArgs> UserRegistered;

        public UserRegistrationService(IServiceProvider serviceProvider)
        {
            _userServices = serviceProvider.GetRequiredService<IUserService>();
            _countryService = serviceProvider.GetRequiredService<ICountryService>();
            _cityService = serviceProvider.GetRequiredService<ICityService>();
            _ethnicityService = serviceProvider.GetRequiredService<IEthnicityService>();
            _occupationService = serviceProvider.GetRequiredService<IOccupationService>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public void Register(UserRegistrationDto userRegistrationDto)
        {
            var newUser = new User();
            var country = GetCountry(userRegistrationDto.CountryName);
            var city = GetCity(country, userRegistrationDto.CityName, userRegistrationDto.CityId);
            var ethnicity = GetEthnicity(userRegistrationDto.EthnicityId);
            var occupation = GetOccupation(userRegistrationDto.OccupationId);


            newUser.Country = country;
            newUser.City = city;
            newUser.Occupation = occupation;
            newUser.Ethnicity = ethnicity;

            newUser.DateOfBirth = userRegistrationDto.DateOfBirth;

            newUser.Email = userRegistrationDto.Email;
            newUser.Name = userRegistrationDto.Name;
            newUser.Phone = userRegistrationDto.Phone;
            newUser.Surname = userRegistrationDto.Surname;

            newUser.Gender = (GenderEnum)userRegistrationDto.Gender;

            _userServices.Insert(newUser);
            _unitOfWork.SaveChangesAsync();

            UserRegistered?.Invoke(this, new UserRegisteredArgs(newUser));
        }

        private Country GetCountry(string countryName)
        {
            if (CountryExistsInDb(countryName))
                return _countryService.FindAsync(countryName).Result;

            var newCountry = new Country { Name = countryName };

            return newCountry;
        }
        private bool CountryExistsInDb(string countryName)
        {
            return _countryService.ExistsAsync(countryName).Result;
        }

        private City GetCity(Country country, string cityName, int cityId)
        {
            if (CityExistsInDb(cityId))
                return _cityService.FindAsync(cityId).Result;

            var newCity = new City {Id = cityId, Country = country, Name = cityName };

            return newCity;
        }
        private bool CityExistsInDb(int cityId)
        {
            return _cityService.ExistsAsync(cityId).Result;
        }

        private Ethnicity GetEthnicity(int ethnicityId)
        {
            return _ethnicityService.FindAsync(ethnicityId).Result;
        }

        private Occupation GetOccupation(int occupationId)
        {
            return _occupationService.FindAsync(occupationId).Result;
        }
    }

    public class UserRegisteredArgs : EventArgs
    {
        public User User { get; set; }

        public UserRegisteredArgs(User user)
        {
            User = user;
        }
    }
}
