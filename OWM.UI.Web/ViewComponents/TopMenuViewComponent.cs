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
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IUserInformationService _userInformation;

        public TopMenuViewComponent(SignInManager<UserIdentity> signInManager, IUserInformationService userInformation)
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
                    Name = _userInformation.GetUserFirstName(identityId)
                };
                return View(model);
            }
            
            return View(new TopMenuDto());
        }
    }
}