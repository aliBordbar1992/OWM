using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles ="User")]
    public class InvitationsModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;
        private readonly ITeamInvitationsService _invitations;

        public InvitationsModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
            , ITeamsManagerService teamManager
            , ITeamInvitationsService invitations)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
            _invitations = invitations;
        }

        public bool EmptyState { get; set; }
        [BindProperty] public List<UserInvitationsDto> Invitations { get; set; }

        public async Task OnGetAsync()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var profileId = await _userInformation.GetUserProfileIdAsync(identityId);

            await _invitations.GarbageInvitationCollection(profileId);

            EmptyState = !await _invitations.HasInvitations(profileId);

            Invitations = await _invitations.GetInvitations(profileId);

        }

        public async Task<IActionResult> OnPostAsync(int invitationid, int teamid)
        {
            await _invitations.FlagAsRead(invitationid);

            if (!await _teamManager.TeamExists(teamid))
                return NotFound();

            return RedirectToPage("/TeamInfo", new { teamid = teamid });
        }
    }
}