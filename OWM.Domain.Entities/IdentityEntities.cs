using Microsoft.AspNetCore.Identity;

namespace OWM.Domain.Entities
{
        public partial class UserLogin : IdentityUserLogin<string>
        {
        }
        public partial class UserClaim : IdentityUserClaim<string>
        {
        }
        public partial class RoleClaim : IdentityRoleClaim<string>
        {
        }
        public partial class UserToken : IdentityUserToken<string>
        {
        }
}