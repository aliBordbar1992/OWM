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
    [Authorize(Roles = "User")]
    public class TeamManagementModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;
        private readonly IOccupationInformationService _ocpInformationService;
        [BindProperty] public List<MyTeamsListDto> TeamsList { get; set; }

        public TeamManagementModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
            , ITeamsManagerService teamManager
            , IOccupationInformationService ocpInformationService)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
            _ocpInformationService = ocpInformationService;
        }


        public async Task OnGetAsync()
        {
            await SetTeamsList();
        }

        private async Task SetTeamsList()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);
            TeamsList = await _teamManager.GetListOfMyTeams(userInfo.ProfileId);
        }
    }
}