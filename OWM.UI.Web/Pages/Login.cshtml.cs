using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.UI.Web.Dtos;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;

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

        public UserRegistrationDto RegistrationData { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }

        public LoginModel(SignInManager<Domain.Entities.User> signInManager
            , UserManager<Domain.Entities.User> userManager)
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
                return LocalRedirect("/User/News");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            returnUrl = returnUrl ?? Url.Page("/User/NewsFeed");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnGetExternalAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Page("/User/NewsFeed");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return Page();
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return Page();
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
            if (result.Succeeded) return LocalRedirect(returnUrl);

            if (result.IsLockedOut) return RedirectToPage("/Lockout");
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                //LoginProvider = info.LoginProvider;
                return RedirectToPage("Register", "External", new { returnUrl });
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid) return Page();

            if (!await IsEmailConfirmed() && !_notRegistered)
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
                returnUrl = returnUrl ?? Url.Page("/User/NewsFeed");
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