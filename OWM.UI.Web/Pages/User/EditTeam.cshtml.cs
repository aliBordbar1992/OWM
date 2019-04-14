using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles = "User")]
    public class EditTeamModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;

        public EditTeamModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
                ,ITeamsManagerService teamManager)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
        }

        public int ProfileId { get; set; }
        [BindProperty(SupportsGet = true)] public int TeamId { get; set; }
        [BindProperty] public TeamInformationDto TeamInformation { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);

            if (!await CanEditTeam(userInfo.ProfileId))
                return NotFound();

            ProfileId = userInfo.ProfileId;

            TeamInformation = await _teamManager.GetTeamInformation(TeamId, true);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);

            if (!await CanEditTeam(userInfo.ProfileId))
                return LocalRedirect("/Index");

            await _teamManager.DeleteTeam(TeamId);
            return LocalRedirect("/User/Teams/List");
        }

        private async Task<bool> CanEditTeam(int profileId)
        {
            return await _teamManager.IsCreatorOfTeam(TeamId, profileId);
        }
    }
}