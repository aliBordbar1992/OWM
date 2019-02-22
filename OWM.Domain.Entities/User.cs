using System;
using Microsoft.AspNetCore.Identity;

namespace OWM.Domain.Entities
{
    public class User : IdentityUser
    {
        public static User CreateIdentity(string username, string email, string phoneNumber = "")
        {
            var newUser = new User
            {
                UserName = username,
                Email = email,
                EmailConfirmed = false,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false
            };

            return newUser;
        }
    }
}