using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using OWM.Application.Services.Email;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class VerificationModel : PageModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserInformationService _userInformation;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private bool _notRegistered;
        private Domain.Entities.User _user;

        public bool DirectVisit;
        public bool EmailAlreadyConfirmed;
        public bool UserNotFound;
        public string Message;

        public VerificationModel(IServiceProvider serviceProvider
            , IUserInformationService userInformation
            , UserManager<Domain.Entities.User> userManager
            , SignInManager<Domain.Entities.User> signInManager)
        {
            _serviceProvider = serviceProvider;
            _userInformation = userInformation;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
           DirectVisit = false;
            if (userId == null || code == null)
            {
                DirectVisit = true;

                EmailAlreadyConfirmed = IsEmailConfirmed(userId).Result;
                if (EmailAlreadyConfirmed)
                {
                    await _signInManager.SignInAsync(_user, true);
                    RedirectToPage("/User/NewsFeed");
                }

                if (_notRegistered) return LocalRedirect("/Index");

                Message = "Your account is not verified. Please check your email to verify your account.";
                return Page();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                UserNotFound = true;
                Message = "Unable to find user.";
                return Page();
            }
            UserNotFound = false;

            var result = await _userManager.ConfirmEmailAsync(user, code);
            EmailAlreadyConfirmed = result.Succeeded;
            Message = !EmailAlreadyConfirmed
                ? "Error confirming email"
                : "Your account has been successfully verified.";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                DirectVisit = false;
                UserNotFound = true;
                Message = "Unable to find user.";
                return Page();
            }

            var userFirstName = await _userInformation.GetUserFirstNameAsync(userId);
            SendVerificationEmail(user, userFirstName);
            return LocalRedirect("/Verify");
        }

        private void SendVerificationEmail(Domain.Entities.User identity, string name)
        {
            var code = _userManager.GenerateEmailConfirmationTokenAsync(identity).Result;
            var callbackUrl = Url.Page(
                "/Verify",
                pageHandler: null,
                values: new { userId = identity.Id, code = code },
                protocol: Request.Scheme);
            string encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            var hostingEnv = _serviceProvider.GetRequiredService<IHostingEnvironment>();

            VerifyEmailEmailSender emailSender = new VerifyEmailEmailSender(hostingEnv, name, identity.Email, encodedUrl);
            emailSender.Send();
        }

        private async Task<bool> IsEmailConfirmed(string id)
        {
            _user = await _userManager.FindByIdAsync(id);
            if (_user == null)
            {
                _notRegistered = true;
                return false;
            }

            _notRegistered = false;
            return await _userManager.IsEmailConfirmedAsync(_user);
        }
    }
}