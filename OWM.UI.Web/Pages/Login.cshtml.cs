using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Domain.Entities;
using OWM.UI.Web.Dtos;
using System.Threading.Tasks;

namespace OWM.UI.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly UserManager<UserIdentity> _userManager;

        public LoginModel(SignInManager<UserIdentity> signInManager, UserManager<UserIdentity> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        [BindProperty]
        public LoginDto Input { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    if (IsEmailConfirmed().Result)
                        returnUrl = returnUrl ?? Url.Content("/User/NewsFeed");
                    else
                        returnUrl = Url.Content("/Verify");
                    return LocalRedirect(returnUrl);
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            return Page();
        }

        private async Task<bool> IsEmailConfirmed()
        {
            string identityName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(identityName);
            if (user == null) return false;

            return await _userManager.IsEmailConfirmedAsync(user);
        }
    }
}