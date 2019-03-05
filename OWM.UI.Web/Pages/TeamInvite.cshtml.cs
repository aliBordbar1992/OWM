using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace OWM.UI.Web.Pages
{
    public class TeamInviteModel : PageModel
    {
        private readonly ITeamInvitationsService _invitations;
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly ITeamsManagerService _teamManager;

        public TeamInviteModel(ITeamInvitationsService invitations
            , SignInManager<Domain.Entities.User> signInManager
            , ITeamsManagerService teamManager)
        {
            _invitations = invitations;
            _signInManager = signInManager;
            _teamManager = teamManager;
        }

        [BindProperty] public ProfileInformationDto MemberInformation { get; set; }
        [BindProperty] public TeamInformationDto TeamInformation { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (!_invitations.TryVerifyToken(id, out var key))
                return NotFound();
            
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToPage("/TeamInfo", new {teamid = _teamManager.GetTeamId(key.TeamGuid)});
            }

            MemberInformation = await _teamManager.GetTeamMemberProfileInformation(key.SenderId);
            TeamInformation = await _teamManager.GetTeamInformation(key.TeamGuid, false);

            return Page();
        }
    }
}