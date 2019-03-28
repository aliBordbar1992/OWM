using System;

namespace OWM.Application.Services.Exceptions
{
    public class UserNotFoundException<TData> : SystemException
    {
        public UserNotFoundException()
        {
        }
        public UserNotFoundException(string displayMessage, Exception e, TData data) : base(displayMessage)
        {
        }
    }
}
