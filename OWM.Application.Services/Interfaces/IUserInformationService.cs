using System;
using System.Collections.Generic;
using OWM.Application.Services.Dtos;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Interfaces
{
    public interface IUserInformationService
    {

        UserInformationDto GetUserInformation(string identityId);
        string GetUserFirstName(string identityId);
        string GetUserSurname(string identityId);
        City GetUserCity(string identityId);
        DateTime GetUserDateOfBirth(string identityId);
        string GetUserOccupation(string identityId);
        string GetUserEthnicity(string identityId);
        List<Interest> GetUserInterests(string identityId);

        UserInformationDto GetUserInformation(int userId);
        string GetUserFirstName(int userId);
        string GetUserSurnam(int userId);
        City GetUserCity(int userId);
        DateTime GetUserDateOfBirth(int userId);
        string GetUserOccupation(int userId);
        string GetUserEthnicity(int userId);
        List<Interest> GetUserInterests(int userId);
    }
}