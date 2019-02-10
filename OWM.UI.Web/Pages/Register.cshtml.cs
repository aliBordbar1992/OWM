using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Email;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserVerificationService _userVerificationService;
        private readonly IServiceProvider _serviceProvider;
        public List<SelectListItem> EthnicityOptions;
        public List<SelectListItem> OccupationOptions;
        public RegisterModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userRegistrationService = serviceProvider.GetRequiredService<IUserRegistrationService>();
            _userVerificationService = serviceProvider.GetRequiredService<IUserVerificationService>();
             EthnicityOptions = new List<SelectListItem>();
             OccupationOptions = new List<SelectListItem>();
        }
        public void OnGet()
        {
           FillDropdowns();
        }
        
        [BindProperty]
        public UserRegistrationDto RegistrationData { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns();
                return Page();
            }
            FillDropdowns();
            _userRegistrationService.UserRegistered += SendVerificationEmail;

            await _userRegistrationService.Register(RegistrationData);
            return RedirectToPage("/Verify");
        }

        public void FillDropdowns()
        {
            EthnicityOptions = _userRegistrationService.GetEthnicities().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;

            OccupationOptions = _userRegistrationService.GetOccupations().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }

        public void SendVerificationEmail(object sender, UserRegisteredArgs e)
        {
            if (!e.User.EmailVerified)
            {
                var hostingEnv = _serviceProvider.GetRequiredService<IHostingEnvironment>();
                var verification = _userVerificationService.CreateEmailVerificationCode(e.User.Id).Result;

                string link = TemplateHelper.GetVerifyLink(verification.VerificatonCode.ToString());
                VerificationEmailSender emailSender = new VerificationEmailSender(hostingEnv, e.User.User.Name, e.User.Email, link);
                emailSender.Send();
            }
        }
    }
}
