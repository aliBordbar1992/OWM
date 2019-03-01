using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.UI.Web.Dtos;

namespace OWM.UI.Web.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserInformationService _userInformation;

        public TopMenuViewComponent(SignInManager<User> signInManager, IUserInformationService userInformation)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_signInManager.IsSignedIn((ClaimsPrincipal) User))
            {
                string identityId = _signInManager.UserManager.GetUserId((ClaimsPrincipal) User);

                var model = new TopMenuDto
                {
                    IsSignedIn = true,
                    Name = await _userInformation.GetUserFirstNameAsync(identityId)
                };
                return View(model);
            }
            
            return View(new TopMenuDto
            {
                IsSignedIn = false,
                Name = ""
            });
        }
    }
}