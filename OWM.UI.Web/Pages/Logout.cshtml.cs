using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OWM.UI.Web.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;

        public LogoutModel(SignInManager<Domain.Entities.User> signInManager)
        {
            _signInManager = signInManager;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return LocalRedirect("/Index");
            }
        }
    }
}