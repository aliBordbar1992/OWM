using System;
using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.Exceptions
{
    public class CompleteMilesFailedException : SystemException
    {
        public CompleteMilesFailedException(Exception e, PledgeMilesDto data)
        {
        }
        public CompleteMilesFailedException(string displayMessage, Exception e, PledgeMilesDto data) : base(displayMessage)
        {
        }
    }
}