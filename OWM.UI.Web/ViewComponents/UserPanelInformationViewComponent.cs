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
        private readonly SignInManager<User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamInvitationsService _invitations;

        public UserPanelInformationViewComponent(SignInManager<User> signInManager
            , IUserInformationService userInformation
            , ITeamInvitationsService invitations)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            this._invitations = invitations;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_signInManager.IsSignedIn((ClaimsPrincipal)User))
            {
                string identityId = _signInManager.UserManager.GetUserId((ClaimsPrincipal)User);
                var info = await _userInformation.GetUserProfileInformationAsync(identityId);
                info.HasInvitations = _invitations.HasInvitations(info.ProfileId);
                return View("/Pages/User/Shared/Components/UserPanelInformation/Default.cshtml", info);
            }
            else
                return View("/Pages/Login.cshtml");
        }
    }
}