using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class EmailVerificationService : Service<EmailVerification>, IEmailVerificationService
    {
        public EmailVerificationService(ITrackableRepository<EmailVerification> repository) : base(repository)
        {
        }
    }
}
