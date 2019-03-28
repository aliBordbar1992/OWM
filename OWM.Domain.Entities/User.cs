using Microsoft.AspNetCore.Identity;

namespace OWM.Domain.Entities
{
    public class User : IdentityUser
    {
        public static User CreateIdentity(string username, string email, bool verifiedEmail, string phoneNumber = "")
        {
            var newUser = new User
            {
                UserName = username,
                Email = email,
                EmailConfirmed = verifiedEmail,
                PhoneNumber = phoneNumber,
                PhoneNumberConfirmed = false
            };

            return newUser;
        }
    }
}