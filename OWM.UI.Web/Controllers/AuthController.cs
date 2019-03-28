using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace OWM.UI.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn(string provider)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, provider);
        }
    }
}