using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace OWM.Application.Services.AppConfigs
{
    public static class AppStartupConfigs
    {
        public static void AddApplicationConfigs(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITrackableRepository<City>, TrackableRepository<City>>();
            services.AddScoped<ITrackableRepository<Country>, TrackableRepository<Country>>();
            services.AddScoped<ITrackableRepository<Ethnicity>, TrackableRepository<Ethnicity>>();
            services.AddScoped<ITrackableRepository<Occupation>, TrackableRepository<Occupation>>();
            services.AddScoped<ITrackableRepository<Profile>, TrackableRepository<Profile>>();
            services.AddScoped<ITrackableRepository<Interest>, TrackableRepository<Interest>>();
            services.AddScoped<ITrackableRepository<Team>, TrackableRepository<Team>>();
            services.AddScoped<ITrackableRepository<MilesPledged>, TrackableRepository<MilesPledged>>();

            services.AddScoped<ICityService, CityService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IEthnicityService, EthnicityService>();
            services.AddScoped<IOccupationService, OccupationService>();
            services.AddScoped<IProfileService, UserService>();
            services.AddScoped<IInterestService, InterestService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IMilesPledgedService, MilesPledgedService>();

            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IUserInformationService, UserInformationService>();
            services.AddScoped<ITeamsManagerService, TeamsManagerService>();
            services.AddScoped<IEthnicityInformationService, EthnicityInformationService>();
            services.AddScoped<IOccupationInformationService, OccupationInformationService>();
        }
    }
}
