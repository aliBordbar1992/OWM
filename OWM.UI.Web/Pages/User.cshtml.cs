using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services;
using OWM.Domain.Services.Interfaces;
using OWM.UI.Web.Dtos;
using URF.Core.Abstractions;

namespace OWM.UI.Web.Pages
{
    public class UserModel : PageModel
    {
        private readonly IUserRegistrationService _registrationService;


        [BindProperty]
        public UserRegistrationDto MyUser { get; set; }

        public UserModel(IUserRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _registrationService.Register(MyUser);
            return RedirectToPage("/Index");
        }
    }
}