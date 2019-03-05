using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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

        public void OnGet()
        {
           FillDropdowns();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/Verify");
            FillDropdowns();

            if (ModelState.IsValid)
            {
                _succeeded = true;

                _userRegistrationService.UserRegistered += SendVerificationEmail;
                _userRegistrationService.UserRegistered += SetUserId;
                _userRegistrationService.UserRegistered += SetInvitationsForThisEmail;

                _userRegistrationService.RegisterFailed += RegisterFailed;

                await _userRegistrationService.Register(RegistrationData);
                if (_succeeded) return LocalRedirect(returnUrl + $"?userid={_uId}");
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
