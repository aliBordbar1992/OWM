using Microsoft.AspNetCore.Identity;

namespace OWM.Domain.Entities
{
    public class Role : IdentityRole
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }
    }
}