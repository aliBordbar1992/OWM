using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles = "User")]
    public class EditProfileModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserInformationService _userInformation;
        private readonly IEthnicityInformationService _ethnicityInformation;
        private bool _succeeded;

        public UserInformationDto UserInformationDto { get; set; }
        public List<SelectListItem> EthnicityOptions;
        [BindProperty] public InputModel Input { get; set; }
        public const string MessageKey = nameof(MessageKey);


        public class InputModel
        {
            [Required(ErrorMessage = "City is Required")]
            public int? CityId { get; set; }

            [Required(ErrorMessage = "Ethnicity is Required")]
            public int? EthnicityId { get; set; }

            [Required(ErrorMessage = "Gender is Required")]
            public int? Gender { get; set; }

            [Required(ErrorMessage = "Name is Required")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Surname is Required")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Mobile no. is required")]
            [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
            public string Phone { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Country is required")]
            public string CountryName { get; set; }

            [Required(ErrorMessage = "City is Required")]
            public string CityName { get; set; }

            [Required(ErrorMessage = "Enter at least one interest")]
            public string Interest { get; set; }

            public List<Interest> Interests => string.IsNullOrEmpty(Interest)
                ? new List<Interest>()
                : Interest.Split(',').Select(x => new Interest { Name = x }).ToList();

            public string UserImage { get; set; }
        }

        public EditProfileModel(SignInManager<Domain.Entities.User> signInManager
            , UserManager<Domain.Entities.User> userManager
            , IServiceProvider serviceProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _userRegistrationService = serviceProvider.GetRequiredService<IUserRegistrationService>();
            _userInformation = _serviceProvider.GetRequiredService<IUserInformationService>();
            _ethnicityInformation = serviceProvider.GetRequiredService<IEthnicityInformationService>();
        }

        public void OnGet()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            UserInformationDto = _userInformation.GetUserInformation(identityId);
            if (Input == null) Input = new InputModel();
            Input.Gender = UserInformationDto.Gender;
            Input.EthnicityId = UserInformationDto.EthnicityId;

            EthnicityOptions = _ethnicityInformation.GetEthnicities().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }

        public async Task OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _succeeded = true;
                var updData = MapToDto(Input);

                _userRegistrationService.UserUpdated += UpdateSuccess;
                _userRegistrationService.UpdateFailed += UpdateFailed;

                await _userRegistrationService.Update(updData);
            }
        }
        private UserRegistrationDto MapToDto(InputModel input)
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            UserInformationDto = _userInformation.GetUserInformation(identityId);

            return new UserRegistrationDto
            {
                Email = UserInformationDto.Email,
                Interest = input.Interest,
                CityId = input.CityId,
                CityName = input.CityName,
                CountryName = input.CountryName,
                EthnicityId = input.EthnicityId,
                Gender = input.Gender,
                Name = input.Name,
                Surname = input.Surname,
                Phone = input.Phone,
                ProfileImageAddress = input.UserImage
            };
        }

        public void UpdateSuccess(object sender, UserUpdatedArgs e)
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            UserInformationDto = _userInformation.GetUserInformation(identityId);
            EthnicityOptions = _ethnicityInformation.GetEthnicities().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;

            TempData[MessageKey] = "Update success!";
        }
        public void UpdateFailed(object sender, UpdateFailedArgs e)
        {
            EthnicityOptions = _ethnicityInformation.GetEthnicities().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;

            _succeeded = false;
            ModelState.AddModelError(string.Empty, e.Exception.Message);
        }

        public async Task<IActionResult> OnPostResetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "/ResetPassword",
                pageHandler: null,
                values: new { code },
                protocol: Request.Scheme);

            string encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            //var hostingEnv = _serviceProvider.GetRequiredService<IHostingEnvironment>();
            //
            //IEmailSender emailSender = new ForgetPasswordEmailSender(hostingEnv, email, encodedUrl);
            //emailSender.Send();

            return Redirect(encodedUrl);
        }
    }
}