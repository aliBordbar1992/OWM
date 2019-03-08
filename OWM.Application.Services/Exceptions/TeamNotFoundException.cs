using System;

namespace OWM.Application.Services.Exceptions
{
    public class TeamNotFoundException<TData> : SystemException
    {
        public TeamNotFoundException()
        {
        }
        public TeamNotFoundException(string displayMessage, Exception e, TData data) : base(displayMessage)
        {
        }
    }
}