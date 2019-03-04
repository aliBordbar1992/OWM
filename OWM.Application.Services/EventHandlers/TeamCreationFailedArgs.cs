using System;

namespace OWM.Application.Services.EventHandlers
{
    public class TeamCreationFailedArgs : EventArgs
    {
        public Exception Exception { get; }

        public TeamCreationFailedArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}