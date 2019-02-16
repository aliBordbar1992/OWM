using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;
using OWM.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IUserService _userService;

        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IEthnicityService _ethnicityService;
        private readonly IOccupationService _occupationService;
        private readonly IUnitOfWork _unitOfWork;

        public event EventHandler<UserRegisteredArgs> UserRegistered;
        public event EventHandler<RegistrationFailedArgs> RegisterFailed;

        public UserRegistrationService(IServiceProvider serviceProvider, UserManager<UserIdentity> userManager)
        {
            _userManager = userManager;
            _countryService = serviceProvider.GetRequiredService<ICountryService>();
            _cityService = serviceProvider.GetRequiredService<ICityService>();
            _ethnicityService = serviceProvider.GetRequiredService<IEthnicityService>();
            _occupationService = serviceProvider.GetRequiredService<IOccupationService>();
            _userService = serviceProvider.GetRequiredService<IUserService>();

            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task Register(UserRegistrationDto userRegistrationDto)
        {
            var newIdentity = UserIdentity.CreateIdentity(userRegistrationDto.Email, userRegistrationDto.Email, userRegistrationDto.Phone);
            var result = await _userManager.CreateAsync(newIdentity, userRegistrationDto.Password);
            if (result.Succeeded)
            {
                var newUser = new User();

                var country = GetCountry(userRegistrationDto.CountryName);
                var city = GetCity(country, userRegistrationDto.CityName, userRegistrationDto.CityId.Value);

                var ethnicity = GetEthnicity(userRegistrationDto.EthnicityId.Value);
                var occupation = GetOccupation(userRegistrationDto.OccupationId.Value);

                newUser.Identity = newIdentity;
                newUser.Country = country;
                newUser.City = city;
                newUser.Occupation = occupation;
                newUser.Ethnicity = ethnicity;

                newUser.DateOfBirth = userRegistrationDto.DateOfBirth;

                newUser.Name = userRegistrationDto.Name;
                newUser.Surname = userRegistrationDto.Surname;
                newUser.Gender = (GenderEnum)userRegistrationDto.Gender.Value;

                _userService.Insert(newUser);

                try
                {
                    await _unitOfWork.SaveChangesAsync();
                    OnUserRegistered(new UserRegisteredArgs(newIdentity, newUser));
                }
                catch (Exception e)
                {
                    if (newUser.TrackingState == TrackingState.Added)
                    {
                        newUser.TrackingState = TrackingState.Deleted;
                        await _unitOfWork.SaveChangesAsync();
                    }

                    await _userManager.DeleteAsync(newIdentity);

                    var error = new IdentityError
                    {
                        Code = "UserSave",
                        Description = "Unable to register. Try again."
                    };
                    OnRegistrationFailed(new RegistrationFailedArgs(new List<IdentityError> { error }));
                }
            }
            else
            {
                OnRegistrationFailed(new RegistrationFailedArgs(result.Errors));
            }
        }


        private Country GetCountry(string countryName)
        {
            if (CountryExistsInDb(countryName))
                return _countryService.Queryable().First(x => x.Name.Equals(countryName));

            var newCountry = new Country { Name = countryName };
            _countryService.Insert(newCountry);

            return newCountry;
        }
        private bool CountryExistsInDb(string countryName) => _countryService.Queryable().Any(x => x.Name.Equals(countryName));

        private City GetCity(Country country, string cityName, int cityId)
        {
            if (CityExistsInDb(cityId))
                return _cityService.Queryable().First(x => x.CustomCityId == cityId);

            var newCity = new City { CustomCityId = cityId, Country = country, Name = cityName };
            _cityService.Insert(newCity);

            return newCity;
        }
        private bool CityExistsInDb(int cityId) => _cityService.Queryable().Any(x => x.CustomCityId == cityId);


        private Ethnicity GetEthnicity(int ethnicityId) => _ethnicityService.FindAsync(ethnicityId).Result;
        private Occupation GetOccupation(int occupationId) => _occupationService.FindAsync(occupationId).Result;

        protected virtual void OnUserRegistered(UserRegisteredArgs e) => UserRegistered?.Invoke(this, e);
        protected virtual void OnRegistrationFailed(RegistrationFailedArgs e) => RegisterFailed?.Invoke(this, e);
        public IAsyncEnumerable<Ethnicity> GetEthnicities() => _ethnicityService.Queryable().ToAsyncEnumerable();
        public IAsyncEnumerable<Occupation> GetOccupations() => _occupationService.Queryable().ToAsyncEnumerable();
    }

    public class UserRegisteredArgs : EventArgs
    {
        public User User { get; }
        public UserIdentity Identity { get; }

        public UserRegisteredArgs(UserIdentity identity, User user)
        {
            User = user;
            Identity = identity;
        }
    }
    public class RegistrationFailedArgs : EventArgs
    {
        public IEnumerable<IdentityError> ResultErrors { get; }

        public RegistrationFailedArgs(IEnumerable<IdentityError> resultErrors)
        {
            ResultErrors = resultErrors;
        }
    }
}
