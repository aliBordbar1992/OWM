using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OWM.Application.Services;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        public List<SelectListItem> EthnicityOptions;
        public List<SelectListItem> OccupationOptions;
        public RegisterModel(IUserRegistrationService userRegistrationService)
        {
             _userRegistrationService = userRegistrationService;
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
            _userRegistrationService.Register(RegistrationData);
            return RedirectToPage("/Register");
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
    }
}
