using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages.User
{
    public class EditProfileModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        public List<SelectListItem> EthnicityOptions;

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
    }
}