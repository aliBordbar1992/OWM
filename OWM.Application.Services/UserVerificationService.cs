using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class UserVerificationService : IUserVerificationService
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IUnitOfWork _unitOfWork;

        public UserVerificationService(IServiceProvider serviceProvider)
        {
            _emailVerificationService = serviceProvider.GetRequiredService<IEmailVerificationService>();
            _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public async Task<EmailVerification> CreateEmailVerificationCode(int userIdentityId)
        {
            DisablePreviousVerificationCodes(userIdentityId);
            var e = EmailVerification.Create(userIdentityId);
            _emailVerificationService.Insert(e);
            await _unitOfWork.SaveChangesAsync();

            return e;
        }

        private void DisablePreviousVerificationCodes(int userIdentityId)
        {
            var codes = _emailVerificationService.Queryable()
                .Where(x => x.UserIdentityId == userIdentityId && !x.Expired);

            if (!codes.Any()) return;

            foreach (var code in codes)
                code.Expired = true;
        }
    }
}