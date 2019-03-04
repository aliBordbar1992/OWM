using System;
using OWM.Domain.Entities;

namespace OWM.Application.Services.EventHandlers
{
    public class UserRegisteredArgs : EventArgs
    {
        public Profile User { get; }
        public User Identity { get; }

        public UserRegisteredArgs(User identity, Profile user)
        {
            User = user;
            Identity = identity;
        }
    }
}