using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class TeamInfoModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;

        public TeamInfoModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
            , ITeamsManagerService teamManager)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
        }

        public int TeamId { get; set; }
        public bool CanJoinTeam { get; set; }
        public bool CanInviteMembers { get; set; }
        [BindProperty] public TeamInformationDto TeamInformation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? teamid)
        {
            if (!teamid.HasValue)
            {
                return LocalRedirect("/User/Teams/List");
            }

            TeamId = teamid.Value;
            if (_signInManager.IsSignedIn(User))
            {
                string identityId = _signInManager.UserManager.GetUserId(User);
                var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);

                CanJoinTeam = await _teamManager.CanJoinTeam(TeamId, userInfo.ProfileId);
                CanInviteMembers = await _teamManager.IsMemberOfTeam(TeamId, userInfo.ProfileId);
            }
            else
                CanJoinTeam = false;


            TeamInformation = await _teamManager.GetTeamInformation(TeamId, false);
            return Page();
        }
    }
}