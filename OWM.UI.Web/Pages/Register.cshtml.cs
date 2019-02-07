using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IUserRegistrationService _userRegistrationService;
        public RegisterModel(IUserRegistrationService userRegistrationService)
        {
             _userRegistrationService = userRegistrationService;
        }
        public void OnGet()
        {
            
        }
        
        [BindProperty]
        public UserRegistrationDto registrationData { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _userRegistrationService.Register(registrationData);
            return RedirectToPage("/Index");
        }
        

    }
}
