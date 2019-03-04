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
        [BindProperty] public TeamInformationDto TeamInformation { get; set; }

        public async Task<IActionResult> OnGet(int? teamid)
        {
            if (!teamid.HasValue)
            {
                return LocalRedirect("/User/Teams/List");
            }

            TeamId = teamid.Value;

            TeamInformation = await _teamManager.GetTeamInformation(TeamId, false);
            return Page();
        }
    }
}