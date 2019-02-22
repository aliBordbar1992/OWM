using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Email;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles = "User")]
    public class EditProfileModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserInformationService _userInformation;
        private readonly IEthnicityInformationService _ethnicityInformation;

        [BindProperty] public UserInformationDto UserInformationDto { get; set; }
        public List<SelectListItem> EthnicityOptions;

        public EditProfileModel(SignInManager<Domain.Entities.User> signInManager
            , UserManager<Domain.Entities.User> userManager
            , IServiceProvider serviceProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _serviceProvider = serviceProvider;
            _userInformation = _serviceProvider.GetRequiredService<IUserInformationService>();
            _ethnicityInformation = serviceProvider.GetRequiredService<IEthnicityInformationService>();
        }
        public void OnGet()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            UserInformationDto = _userInformation.GetUserInformation(identityId);

            EthnicityOptions = _ethnicityInformation.GetEthnicities().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }

        public void OnPost()
        {

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