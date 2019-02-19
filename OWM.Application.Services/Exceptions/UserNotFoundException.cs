using System;
using System.Collections.Generic;
using System.Text;

namespace OWM.Application.Services.Exceptions
{
    public class UserNotFoundException : SystemException
    {
        public UserNotFoundException()
        {
        }
        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}
