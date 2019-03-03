using System;
using System.Collections.Generic;
using System.Linq;
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

        public int TeamId { get; set; }
        [BindProperty] public TeamInformationDto TeamInformation { get; set; }

        public async Task<IActionResult> OnGet(int? teamid)
        {
            if (!teamid.HasValue)
            {
                return LocalRedirect("/User/Teams/List");
            }

            TeamId = teamid.Value;

            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);

            if (!await _teamManager.IsMemberOfTeam(TeamId, userInfo.ProfileId))
                return LocalRedirect("/User/Teams/List");

            TeamInformation = await _teamManager.GetTeamInformation(TeamId);
            return Page();
        }
    }
}