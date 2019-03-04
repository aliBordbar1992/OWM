using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace OWM.Application.Services.EventHandlers
{
    public class RegistrationFailedArgs : EventArgs
    {
        public IEnumerable<IdentityError> ResultErrors { get; }

        public RegistrationFailedArgs(IEnumerable<IdentityError> resultErrors)
        {
            ResultErrors = resultErrors;
        }
    }
}