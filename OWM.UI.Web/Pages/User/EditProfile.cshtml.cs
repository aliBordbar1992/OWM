using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Interfaces;
using OWM.Application.Services.Dtos;

namespace OWM.UI.Web.Pages.User
{
    public class EditProfileModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        public List<SelectListItem> EthnicityOptions;

        [BindProperty] public UserInformationDto UserInformationDto { get; set; }

        public EditProfileModel(IServiceProvider serviceProvider)
        {
            _userRegistrationService = serviceProvider.GetRequiredService<IUserRegistrationService>();
            EthnicityOptions = new List<SelectListItem>();
        }
        public void OnGet()
        {
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