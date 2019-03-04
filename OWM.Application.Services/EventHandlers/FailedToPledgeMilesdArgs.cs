using System;

namespace OWM.Application.Services.EventHandlers
{
    public class FailedToPledgeMilesdArgs : EventArgs
    {
        public Exception Exception { get; }

        public FailedToPledgeMilesdArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}