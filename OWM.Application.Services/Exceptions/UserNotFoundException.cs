using System;
using System.Collections.Generic;
using System.Text;
using OWM.Application.Services.Dtos;

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
