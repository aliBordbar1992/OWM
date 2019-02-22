using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class RegisterTestModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager;

        private string _uId;

        public List<SelectListItem> EthnicityOptions;
        public List<SelectListItem> OccupationOptions;
        public RegisterTestModel(IServiceProvider serviceProvider, UserManager<Domain.Entities.User> userManager,
            SignInManager<Domain.Entities.User> signInManager)
        {
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _signInManager = signInManager;
            _userRegistrationService = serviceProvider.GetRequiredService<IUserRegistrationService>();
            EthnicityOptions = new List<SelectListItem>();
            OccupationOptions = new List<SelectListItem>();
        }
        public void OnGet()
        {
            FillDropdowns();
        }

        private bool _succeeded;

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("/Verify");
            FillDropdowns();

            if (ModelState.IsValid)
            {
                _succeeded = true;

                _userRegistrationService.UserRegistered += SendVerificationEmail;
                _userRegistrationService.UserRegistered += SetUserId;

                _userRegistrationService.RegisterFailed += RegisterFailed;

                await _userRegistrationService.Register(TestData());
                if (_succeeded) return LocalRedirect(returnUrl + $"?userid={_uId}");
            }

            return Page();
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

        public void SetUserId(object sender, UserRegisteredArgs e)
        {
            _uId = e.Identity.Id;
        }
        public void LoginUser(object sender, UserRegisteredArgs e)
        {
            _signInManager.SignInAsync(e.Identity, isPersistent: false);
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

            VerificationEmailSender emailSender = new VerificationEmailSender(hostingEnv, e.User.Name, e.Identity.Email, encodedUrl);
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

        public UserRegistrationDto TestData()
        {
            return new  UserRegistrationDto
            {
                Interest = "test,test2",
                Birthday = "2019-02-02",
                CityId = 4066,
                CityName = "Tehran, TH, Iran",
                ConfirmPassword = "Mo$tashar2",
                CountryName = "Iran",
                Email = "witch_king2011@yahoo.com",
                EthnicityId = 1,
                Gender = 1,
                Name = "ali",
                OccupationId = 1,
                Password = "Mo$tashar2",
                Phone = "09372346281",
                Surname = "Bordbar"
            };
        }
    }
}