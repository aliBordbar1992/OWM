using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Exceptions;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;

namespace OWM.UI.Web.ViewComponents
{
    public class UserPanelInformationViewComponent : ViewComponent
    {
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IUserInformationService _userInformation;

        public UserPanelInformationViewComponent(SignInManager<UserIdentity> signInManager, IUserInformationService userInformation)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_signInManager.IsSignedIn((ClaimsPrincipal)User))
            {
                string identityId = _signInManager.UserManager.GetUserId((ClaimsPrincipal)User);
                var info = _userInformation.GetUserInformation(identityId);
                return View("/Pages/User/Shared/Components/UserPanelInformation/Default.cshtml", info);
            }
            else 
                throw new UserNotFoundException();
        }
    }
}