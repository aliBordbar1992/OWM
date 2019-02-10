using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class exploreModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        public List<SelectListItem> OccupationOptions;

        public exploreModel(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
            OccupationOptions = new List<SelectListItem>();
        }

        public void OnGet()
        {
            FillOccupationDropdown();
        }
        public void FillOccupationDropdown()
        {
            OccupationOptions = _userRegistrationService.GetOccupations().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }
    }
}