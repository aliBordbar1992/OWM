using System;
using Microsoft.AspNetCore.Identity;

namespace OWM.Domain.Entities
{
    public class UserIdentity : IdentityUser
    {
        public static UserIdentity CreateIdentity(string username, string email, string phoneNumber = "")
        {
            var newUserIdentity = new UserIdentity
            {
                UserName = username,
                Email = email,
                EmailConfirmed = false,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false
            };

            return newUserIdentity;
        }
    }
}