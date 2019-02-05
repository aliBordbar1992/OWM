using System;
using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.Interfaces
{
    public interface IUserRegistrationService
    {
        event EventHandler<UserRegisteredArgs> UserRegistered;
        void Register(UserRegistrationDto userRegistrationDto);
    }
}