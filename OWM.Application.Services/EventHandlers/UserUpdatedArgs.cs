using System;
using OWM.Domain.Entities;

namespace OWM.Application.Services.EventHandlers
{
    public class UserUpdatedArgs : EventArgs
    {
        public Profile User { get; }
        public User Identity { get; }

        public UserUpdatedArgs(User identity, Profile user)
        {
            User = user;
            Identity = identity;
        }
    }
}