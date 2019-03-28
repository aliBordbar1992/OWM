﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.EventHandlers;

namespace OWM.Application.Services.Interfaces
{
    public interface IUserRegistrationService
    {
        event EventHandler<UserRegisteredArgs> UserRegistered;
        event EventHandler<RegistrationFailedArgs> RegisterFailed;

        event EventHandler<UserUpdatedArgs> UserUpdated;
        event EventHandler<UpdateFailedArgs> UpdateFailed;

        event EventHandler<string> UserExternalLoginAdded;
        event EventHandler<List<IdentityError>> UserExternalLoginAddFailed;

        Task Register(UserRegistrationDto userRegistrationDto, ExternalLoginInfo info = null);
        Task Update(UserRegistrationDto userRegistrationDto);
        Task AddExternalLogin(ExternalLoginInfo info);
    }
}