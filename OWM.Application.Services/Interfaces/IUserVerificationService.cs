using System.Threading.Tasks;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Interfaces
{
    public interface IUserVerificationService
    {
        Task<EmailVerification> CreateEmailVerificationCode(int userIdentityId);
    }
}