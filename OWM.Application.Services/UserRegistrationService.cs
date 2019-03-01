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
using OWM.Application.Services.Exceptions;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IProfileService _profileService;

        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IEthnicityService _ethnicityService;
        private readonly IOccupationService _occupationService;
        private readonly IInterestService _interestService;
        private readonly IUnitOfWork _unitOfWork;

        public event EventHandler<UserRegisteredArgs> UserRegistered;
        public event EventHandler<RegistrationFailedArgs> RegisterFailed;
        public event EventHandler<UserUpdatedArgs> UserUpdated;
        public event EventHandler<UpdateFailedArgs> UpdateFailed;

        public UserRegistrationService(IServiceProvider serviceProvider, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _countryService = serviceProvider.GetRequiredService<ICountryService>();
            _cityService = serviceProvider.GetRequiredService<ICityService>();
            _ethnicityService = serviceProvider.GetRequiredService<IEthnicityService>();
            _occupationService = serviceProvider.GetRequiredService<IOccupationService>();
            _profileService = serviceProvider.GetRequiredService<IProfileService>();
            _interestService = serviceProvider.GetRequiredService<IInterestService>();

            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task Register(UserRegistrationDto userRegistrationDto)
        {
            var user = User.CreateIdentity(userRegistrationDto.Email, userRegistrationDto.Email, userRegistrationDto.Phone);
            var identityResult = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, ApplicationRoles.User);

                var profile = new Profile();

                var country = GetCountry(userRegistrationDto.CountryName);
                var city = GetCity(country, userRegistrationDto.CityName, userRegistrationDto.CityId.Value);

                var ethnicity = GetEthnicity(userRegistrationDto.EthnicityId.Value);
                var occupation = GetOccupation(userRegistrationDto.OccupationId.Value);
                var interests = GetInterests(userRegistrationDto.Interests);

                profile.Identity = user;
                profile.Country = country;
                profile.City = city;
                profile.Occupation = occupation;
                profile.Ethnicity = ethnicity;
                profile.Interests = interests;

                profile.DateOfBirth = userRegistrationDto.DateOfBirth.Value;

                profile.Name = userRegistrationDto.Name;
                profile.Surname = userRegistrationDto.Surname;
                profile.Gender = (GenderEnum)userRegistrationDto.Gender.Value;

                _profileService.Insert(profile);

                try
                {
                    await _unitOfWork.SaveChangesAsync();
                    OnUserRegistered(new UserRegisteredArgs(user, profile));
                }
                catch (Exception e)
                {
                    if (_profileService.ExistsAsync(profile.Id).Result)
                    {
                        _profileService.Delete(profile);
                        await _unitOfWork.SaveChangesAsync();
                        await _userManager.DeleteAsync(user);
                    }


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
                OnRegistrationFailed(new RegistrationFailedArgs(identityResult.Errors));
            }
        }

        public async Task Update(UserRegistrationDto userRegistrationDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userRegistrationDto.Email);
                if (user == null) throw new UserNotFoundException<UserRegistrationDto>($"No user found related to {userRegistrationDto.Email}", null, userRegistrationDto);

                var profile = _profileService.Queryable().Single(x => x.Identity.Id == user.Id);

                if (profile.Country.Name != userRegistrationDto.CountryName)
                    profile.Country = GetCountry(userRegistrationDto.CountryName);

                if (profile.City.Id != userRegistrationDto.CityId.Value)
                    profile.City = GetCity(profile.Country, userRegistrationDto.CityName, userRegistrationDto.CityId.Value);

                if (profile.Ethnicity.Id != userRegistrationDto.EthnicityId)
                    profile.Ethnicity = GetEthnicity(userRegistrationDto.EthnicityId.Value);

                RemoveInterests(profile.Interests.ToList());
                profile.Interests = GetInterests(userRegistrationDto.Interests);

                profile.Name = userRegistrationDto.Name;
                profile.Surname = userRegistrationDto.Surname;
                profile.Gender = (GenderEnum)userRegistrationDto.Gender.Value;
                profile.ProfileImageUrl =
                    profile.ProfileImageUrl == userRegistrationDto.ProfileImageAddress ||
                    string.IsNullOrEmpty(userRegistrationDto.ProfileImageAddress)
                        ? profile.ProfileImageUrl
                        : userRegistrationDto.ProfileImageAddress;
                user.PhoneNumber = userRegistrationDto.Phone;

                await _unitOfWork.SaveChangesAsync();
                OnUserUpdated(new UserUpdatedArgs(user, profile));
            }
            catch (Exception e)
            {
                OnUpdateFailed(new UpdateFailedArgs(e));
                throw;
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

        private List<Interest> GetInterests(List<Interest> interests)
        {
            foreach (var interest in interests)
            {
                _interestService.Insert(interest);
            }

            return interests;
        }
        private void RemoveInterests(List<Interest> interests)
        {
            foreach (var interest in interests)
            {
                _interestService.Delete(interest);
            }
        }

        private Ethnicity GetEthnicity(int ethnicityId) => _ethnicityService.FindAsync(ethnicityId).Result;
        private Occupation GetOccupation(int occupationId) => _occupationService.FindAsync(occupationId).Result;

        protected virtual void OnUserRegistered(UserRegisteredArgs e) => UserRegistered?.Invoke(this, e);
        protected virtual void OnRegistrationFailed(RegistrationFailedArgs e) => RegisterFailed?.Invoke(this, e);

        protected virtual void OnUserUpdated(UserUpdatedArgs e) => UserUpdated?.Invoke(this, e);
        protected virtual void OnUpdateFailed(UpdateFailedArgs e) => UpdateFailed?.Invoke(this, e);
    }

    public class UserUpdatedArgs : EventArgs
    {
        public Profile User { get; }
        public User Identity { get; }

        public UserUpdatedArgs(User identity, Profile user)
        {
            User = user;
            Identity = identity;
        }
    }
    public class UpdateFailedArgs : EventArgs
    {
        public Exception Exception { get; }

        public UpdateFailedArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    public class UserRegisteredArgs : EventArgs
    {
        public Profile User { get; }
        public User Identity { get; }

        public UserRegisteredArgs(User identity, Profile user)
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
