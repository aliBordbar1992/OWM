using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Interfaces
{
    public interface IUserRegistrationService
    {
        event EventHandler<UserRegisteredArgs> UserRegistered;
        event EventHandler<RegistrationFailedArgs> RegisterFailed;


        Task Register(UserRegistrationDto userRegistrationDto);
        IAsyncEnumerable<Ethnicity> GetEthnicities();
        IAsyncEnumerable<Occupation> GetOccupations();
    }
}