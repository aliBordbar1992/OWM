using System;

namespace OWM.Application.Services.EventHandlers
{
    public class UpdateFailedArgs : EventArgs
    {
        public Exception Exception { get; }

        public UpdateFailedArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}