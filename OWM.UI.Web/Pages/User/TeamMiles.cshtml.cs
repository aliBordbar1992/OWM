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
    public class TeamMilesModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;
        private readonly ITeamMilesService _teamMilesService;

        public int TeamId { get; set; }
        public int ProfileId { get; set; }
        public TeamMilesInformationDto MilesInformation { get; set; }

        public TeamMilesModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
            , ITeamsManagerService teamManager
            , ITeamMilesService teamMilesService)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
            _teamMilesService = teamMilesService;
        }


        public async Task<IActionResult> OnGet(int? teamId)
        {
            if (!teamId.HasValue)
                return LocalRedirect("/User/Teams/List");

            TeamId = teamId.Value;
            if (!await _teamManager.TeamExists(teamId.Value))
                return NotFound();

            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);

            if (!await CanEditTeamMiles(userInfo.ProfileId))
                return NotFound();

            ProfileId = userInfo.ProfileId;

            MilesInformation = await _teamMilesService.GetTeamMilesInformation(TeamId, ProfileId);
            return Page();
        }



        private async Task<bool> CanEditTeamMiles(int profileId)
        {
            return await _teamManager.IsMemberOfTeam(TeamId, profileId);
        }

        
    }
}