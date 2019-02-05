using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.Interfaces
{
    public interface IUserRegistrationService
    {
        void Register(UserRegistrationDto userRegistrationDto);
    }
}