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
        public UserRegistrationDto registrationData { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                FillDropdowns();
                return Page();
            }
            FillDropdowns();
            _userRegistrationService.Register(registrationData);
            return RedirectToPage("/Register");
        }

        public void FillDropdowns()
        {
            var ethlist = _userRegistrationService.GetEthnicities().ToList();
            var occList = _userRegistrationService.GetOccupations().ToList();
            foreach (var eth in ethlist.Result)
            {
                EthnicityOptions.Add(new SelectListItem()
                {
                    Value = eth.Id.ToString(),
                    Text = eth.Name.ToString()
                });
            }

            foreach (var occ in occList.Result)
            {
                OccupationOptions.Add(new SelectListItem()
                {
                    Text = occ.Name.ToString(),
                    Value = occ.Id.ToString()
                });
            }
        }
    }
}
