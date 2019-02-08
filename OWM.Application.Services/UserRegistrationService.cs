using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;
using OWM.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly IUserService _userService;
        private readonly IUserIdentityService _userIdentityService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IEthnicityService _ethnicityService;
        private readonly IOccupationService _occupationService;
        private readonly IUnitOfWork _unitOfWork;

        public event EventHandler<UserRegisteredArgs> UserRegistered;

        public UserRegistrationService(IServiceProvider serviceProvider)
        {
            _countryService = serviceProvider.GetRequiredService<ICountryService>();
            _cityService = serviceProvider.GetRequiredService<ICityService>();
            _ethnicityService = serviceProvider.GetRequiredService<IEthnicityService>();
            _occupationService = serviceProvider.GetRequiredService<IOccupationService>();
            _userService = serviceProvider.GetRequiredService<IUserService>();
            _userIdentityService = serviceProvider.GetRequiredService<IUserIdentityService>();

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

            newUser.Name = userRegistrationDto.Name;
            newUser.Surname = userRegistrationDto.Surname;

            newUser.Gender = (GenderEnum)userRegistrationDto.Gender;

            var newIdentity = UserIdentity.CreateIdentity(userRegistrationDto.Email, userRegistrationDto.Password,
                userRegistrationDto.Email, newUser, userRegistrationDto.Phone);

            _userIdentityService.Insert(newIdentity);
            _userService.Insert(newUser);
            _unitOfWork.SaveChangesAsync();

            OnUserRegistered(new UserRegisteredArgs(newIdentity));
        }

        private Country GetCountry(string countryName)
        {
            if (CountryExistsInDb(countryName))
                return _countryService.Queryable().First(x => x.Name.Equals(countryName));

            var newCountry = new Country { Name = countryName };
            _countryService.Insert(newCountry);

            return newCountry;
        }
        private bool CountryExistsInDb(string countryName)
        {
            return _countryService.Queryable().Any(x => x.Name.Equals(countryName));
        }

        private City GetCity(Country country, string cityName, int cityId)
        {
            if (CityExistsInDb(cityId))
                return _cityService.Queryable().First(x => x.CustomCityId == cityId);

            var newCity = new City {CustomCityId = cityId, Country = country, Name = cityName };
            _cityService.Insert(newCity);

            return newCity;
        }
        private bool CityExistsInDb(int cityId)
        {
            return _cityService.Queryable().Any(x => x.CustomCityId == cityId);
        }

        private Ethnicity GetEthnicity(int ethnicityId)
        {
            return _ethnicityService.FindAsync(ethnicityId).Result;
        }

        private Occupation GetOccupation(int occupationId)
        {
            return _occupationService.FindAsync(occupationId).Result;
        }

        protected virtual void OnUserRegistered(UserRegisteredArgs e)
        {
            UserRegistered?.Invoke(this, e);
        }

        public IAsyncEnumerable<Ethnicity> GetEthnicities()
        {
            return _ethnicityService.Queryable().ToAsyncEnumerable();
        }

        public IAsyncEnumerable<Occupation> GetOccupations()
        {
            return _occupationService.Queryable().ToAsyncEnumerable();
        }
    }

    public class UserRegisteredArgs : EventArgs
    {
        public UserIdentity User { get; set; }

        public UserRegisteredArgs(UserIdentity user)
        {
            User = user;
        }
    }
}
