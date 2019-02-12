using System;
using Microsoft.AspNetCore.Identity;

namespace OWM.Domain.Entities
{
    public class UserIdentity : IdentityUser
    {
        public virtual User User { get; private set; }
        public static UserIdentity CreateIdentity(string username, string email, User newUser, string phoneNumber = "")
        {
            var newUserIdentity = new UserIdentity
            {
                UserName = username,
                Email = email,
                EmailConfirmed = false,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false,
                User = newUser
            };

            return newUserIdentity;
        }
    }
}