using System;
using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.Exceptions
{
    public class PledgedMilesFailedException : SystemException
    {
        public PledgedMilesFailedException(Exception e, PledgeMilesDto data)
        {
        }
        public PledgedMilesFailedException(string displayMessage, Exception e, PledgeMilesDto data) : base(displayMessage)
        {
        }
    }
}