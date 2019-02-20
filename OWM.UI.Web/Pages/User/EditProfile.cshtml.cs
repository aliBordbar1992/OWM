using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Interfaces;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Exceptions;
using OWM.Domain.Entities;

namespace OWM.UI.Web.Pages.User
{
    public class EditProfileModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        public List<SelectListItem> EthnicityOptions;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IUserInformationService _userInformation;

        [BindProperty] public UserInformationDto UserInformationDto { get; set; }

        public EditProfileModel(IServiceProvider serviceProvider, SignInManager<UserIdentity> signInManager,
            IUserInformationService userInformation)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _userRegistrationService = serviceProvider.GetRequiredService<IUserRegistrationService>();
            EthnicityOptions = new List<SelectListItem>();
        }
        public void OnGet()
        {
            if (_signInManager.IsSignedIn((ClaimsPrincipal)User))
            {
                string identityId = _signInManager.UserManager.GetUserId((ClaimsPrincipal)User);
                UserInformationDto = _userInformation.GetUserInformation(identityId);
            }
            else throw new UserNotFoundException();

            EthnicityOptions = _userRegistrationService.GetEthnicities().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }

        public void OnPost()
        {

        }
    }
}