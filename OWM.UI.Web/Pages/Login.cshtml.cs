using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Domain.Entities;
using OWM.UI.Web.Dtos;
using System.Threading.Tasks;

namespace OWM.UI.Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private string _uId;
        private bool _notRegistered;

        [BindProperty]
        public LoginDto Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public string ReturnUrl { get; set; }

        public LoginModel(SignInManager<Domain.Entities.User> signInManager, UserManager<Domain.Entities.User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            if (_signInManager.IsSignedIn(User))
                return LocalRedirect("/User/NewsFeed");

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            if (!IsEmailConfirmed().Result && !_notRegistered)
            {
                returnUrl = Url.Content("/Verify" + $"?userid={_uId}");
                return LocalRedirect(returnUrl);
            }

            if (_notRegistered)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                returnUrl = returnUrl ?? Url.Content("/User/NewsFeed");
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        private async Task<bool> IsEmailConfirmed()
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                _notRegistered = true;
                return false;
            }

            _notRegistered = false;
            _uId = user.Id;
            return await _userManager.IsEmailConfirmedAsync(user);
        }
    }
}