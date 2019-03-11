using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.UI.Web.Dtos;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IUserRegistrationService _userRegistration;
        private string _uId;
        private bool _notRegistered;
        private bool _externalSuccess;

        [BindProperty]
        public LoginDto Input { get; set; }

        public UserRegistrationDto RegistrationData { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }

        public LoginModel(SignInManager<Domain.Entities.User> signInManager
            , UserManager<Domain.Entities.User> userManager
            , IUserRegistrationService userRegistration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRegistration = userRegistration;
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
            if (result.Succeeded)
            {
                if (!await IsExternalEmailConfirmed(info) && !_notRegistered)
                {
                    await _signInManager.SignOutAsync();
                    returnUrl = Url.Content("/Verify" + $"?userid={_uId}");
                    return LocalRedirect(returnUrl);
                }

                if (_notRegistered)
                {
                    ModelState.AddModelError(string.Empty, $"Failed to login with {info.LoginProvider}");
                    return Page();
                }

                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut) return RedirectToPage("/Lockout");
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                if (HaveEmailPrincipal(info) && await UserWithEmailExists(info))
                {
                    _externalSuccess = false;
                    _userRegistration.UserExternalLoginAdded += UserExternalLoginAdded;
                    await _userRegistration.AddExternalLogin(info);

                    if (_externalSuccess)
                    {
                        result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
                        if (result.Succeeded)
                        {
                            if (!await IsExternalEmailConfirmed(info) && !_notRegistered)
                            {
                                await _signInManager.SignOutAsync();
                                returnUrl = Url.Content("/Verify" + $"?userid={_uId}");
                                return LocalRedirect(returnUrl);
                            }

                            if (_notRegistered)
                            {
                                ModelState.AddModelError(string.Empty, $"Failed to login with {info.LoginProvider}");
                                return Page();
                            }

                            return LocalRedirect(returnUrl);
                        }
                    }
                }
                return RedirectToPage("Register", "External", new { returnUrl });
            }
        }

        private async Task<bool> UserWithEmailExists(ExternalLoginInfo info)
        {
            string email = info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)
                ? info.Principal.FindFirstValue(ClaimTypes.Email)
                : "";
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        private bool HaveEmailPrincipal(ExternalLoginInfo info)
        {
            return info.Principal.HasClaim(c => c.Type == ClaimTypes.Email);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
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

        private async Task<bool> IsExternalEmailConfirmed(ExternalLoginInfo info)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                _notRegistered = true;
                return false;
            }

            _notRegistered = false;
            _uId = user.Id;
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public void UserExternalLoginAdded(object sender, string e)
        {
            _externalSuccess = true;
        }
        public void UserExternalLoginAddFailed(object sender, List<IdentityError> e)
        {
            foreach (var error in e)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
