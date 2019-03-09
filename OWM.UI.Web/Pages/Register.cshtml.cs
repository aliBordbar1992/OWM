using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Email;
using OWM.Application.Services.EventHandlers;
using OWM.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OWM.UI.Web.Pages
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IEthnicityInformationService _ethnicityInformation;
        private readonly IOccupationInformationService _ocpInformation;
        private readonly ITeamInvitationsService _invitations;
        private string _uId;
        private bool _succeeded;

        [BindProperty] public UserRegistrationDto RegistrationData { get; set; }
        public List<SelectListItem> EthnicityOptions;
        public List<SelectListItem> OccupationOptions;
        [TempData] public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }

        public RegisterModel(IServiceProvider serviceProvider
            , UserManager<Domain.Entities.User> userManager
            , SignInManager<Domain.Entities.User> signInManager)
        {
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _signInManager = signInManager;

            _userRegistrationService = serviceProvider.GetRequiredService<IUserRegistrationService>();
            _ethnicityInformation = serviceProvider.GetRequiredService<IEthnicityInformationService>();
            _ocpInformation = serviceProvider.GetRequiredService<IOccupationInformationService>();
            _invitations = serviceProvider.GetRequiredService<ITeamInvitationsService>();

            EthnicityOptions = new List<SelectListItem>();
            OccupationOptions = new List<SelectListItem>();
        }

        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(User))
                return LocalRedirect("/User/News");

            FillDropdowns();
            return Page();
        }

        public async Task<IActionResult> OnGetExternalAsync(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
                return LocalRedirect("/User/News");
            FillDropdowns();


            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return Page();
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded) return LocalRedirect("/User/News");

            // If the user does not have an account, then ask the user to create an account.
            ReturnUrl = returnUrl;
            FillRegistrationInformation(info);
            return Page();
        }

        private void FillRegistrationInformation(ExternalLoginInfo info, UserRegistrationDto dto = null)
        {
            string email = info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)
                ? info.Principal.FindFirstValue(ClaimTypes.Email)
                : "";

            string name = info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName)
                ? info.Principal.FindFirstValue(ClaimTypes.GivenName)
                : "";

            string surname = info.Principal.HasClaim(c => c.Type == ClaimTypes.Surname)
                ? info.Principal.FindFirstValue(ClaimTypes.Surname)
                : "";

            string phone = info.Principal.HasClaim(c => c.Type == ClaimTypes.MobilePhone)
                ? info.Principal.FindFirstValue(ClaimTypes.MobilePhone)
                : "";

            int? gender = info.Principal.HasClaim(c => c.Type == ClaimTypes.Gender)
                ? info.Principal.FindFirstValue(ClaimTypes.Gender).Equals("male") ? 1 : 0
                : (int?)null;

            string profilePicture = info.Principal.HasClaim(c => c.Type == ClaimTypes.Thumbprint)
                ? info.Principal.FindFirstValue(ClaimTypes.Thumbprint)
                : "";

            bool verifiedEmail = info.Principal.HasClaim(c => c.Type == "verified_email") &&
                                 (info.Principal.FindFirstValue("verified_email").Equals("True"));
            if (dto == null)
            {
                RegistrationData = new UserRegistrationDto
                {
                    Email = email,
                    Name = name,
                    Surname = surname,
                    Phone = phone,
                    Gender = gender,
                    ProfileImageAddress = profilePicture,
                    VerfiedEmail = verifiedEmail
                };
            }
            else
            {
                dto.ProfileImageAddress = profilePicture;
                dto.VerfiedEmail = verifiedEmail;
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Page("/Verify");
            FillDropdowns();

            if (!ModelState.IsValid) return Page();

            _succeeded = true;

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info != null) FillRegistrationInformation(info, RegistrationData);

            if (!RegistrationData.VerfiedEmail) _userRegistrationService.UserRegistered += SendVerificationEmail;
            _userRegistrationService.UserRegistered += SetUserId;
            _userRegistrationService.UserRegistered += SetInvitationsForThisEmail;

            _userRegistrationService.RegisterFailed += RegisterFailed;

            await _userRegistrationService.Register(RegistrationData, info);
            if (_succeeded && RegistrationData.VerfiedEmail)
            {
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
                if (result.Succeeded) return LocalRedirect("/User/News");
            }
            else if (_succeeded)
            {
                return LocalRedirect(returnUrl + $"?userid={_uId}");
            }

            return Page();
        }

        public void FillDropdowns()
        {
            EthnicityOptions = _ethnicityInformation.GetEthnicities().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;

            OccupationOptions = _ocpInformation.GetOccupations().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }

        public void SetUserId(object sender, UserRegisteredArgs e)
        {
            _uId = e.Identity.Id;
        }
        public void SetInvitationsForThisEmail(object sender, UserRegisteredArgs e)
        {
            _invitations.UpdateInvitations(e.Identity.Email, e.User.Id);
        }
        public void SendVerificationEmail(object sender, UserRegisteredArgs e)
        {
            var code = _userManager.GenerateEmailConfirmationTokenAsync(e.Identity).Result;
            var callbackUrl = Url.Page(
                "/Verify",
                pageHandler: null,
                values: new { userId = e.Identity.Id, code = code },
                protocol: Request.Scheme);
            string encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            var hostingEnv = _serviceProvider.GetRequiredService<IHostingEnvironment>();

            VerifyEmailEmailSender emailSender = new VerifyEmailEmailSender(hostingEnv, e.User.Name, e.Identity.Email, encodedUrl);
            emailSender.Send();
        }
        public void RegisterFailed(object sender, RegistrationFailedArgs e)
        {
            _succeeded = false;
            foreach (var error in e.ResultErrors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
