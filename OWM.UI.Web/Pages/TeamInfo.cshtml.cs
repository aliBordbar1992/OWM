using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.EventHandlers;
using OWM.Application.Services.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OWM.UI.Web.Pages
{
    public class TeamInfoModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;
        private readonly ITeamMilesService _teamMiles;
        private readonly ITeamMessageBoardService _msgBoardService;

        public TeamInfoModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
            , ITeamsManagerService teamManager
            , ITeamMilesService teamMiles
            , ITeamMessageBoardService msgBoardService)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
            _teamMiles = teamMiles;
            _msgBoardService = msgBoardService;
        }

        public int TeamId { get; set; }
        public CanJoinTeamDto CanJoinTeam { get; set; }
        //public bool CanInviteMembers { get; set; }
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
                CanJoinTeam.IsLoggedIn = true;
                //CanInviteMembers = await _teamManager.IsMemberOfTeam(TeamId, userInfo.ProfileId);
            }
            else
            {
                CanJoinTeam = new CanJoinTeamDto
                {
                    IsLoggedIn = false,
                    OccupationFilter = true,
                    IsAlreadyMember = false,
                    FinalResult = false,
                    AgeRangeMatch = false,
                    TeamIsClosed = true,
                    OccupationsMatch = false
                };
            }

            if (!await _teamManager.TeamExists(TeamId))
                return NotFound();

            TeamInformation = await _teamManager.GetTeamInformation(TeamId, false);

            return Page();
        }

        public bool JoinedSuccess { get; set; }
        public const string MessageKey = nameof(MessageKey);
        [BindProperty] public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Enter miles you want to pledge to this team")]
            [AssertThat("MilesPledged > 0", ErrorMessage = "Miles pledged must be greater than 0.")]
            public float MilesPledged { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(int teamId)
        {
            TeamId = teamId;

            if (!_signInManager.IsSignedIn(User)) return new UnauthorizedResult();

            string identityId = _signInManager.UserManager.GetUserId(User);
            var profileId = _userInformation.GetUserProfileIdAsync(identityId).Result;

            CanJoinTeam = await _teamManager.CanJoinTeam(TeamId, profileId);
            TeamInformation = await _teamManager.GetTeamInformation(TeamId, false);

            if (ModelState.IsValid)
            {
                if (CanJoinTeam.FinalResult)
                {
                    _teamManager.JoinedTeamSuccessfully += JoinedTeamSuccessfully;
                    _teamManager.JoinedTeamSuccessfully += AddToBoardParticipants;
                    _teamManager.JoinTeamFailed += JoinFailed;

                    await _teamManager.JoinTeam(TeamId, profileId);
                    if (JoinedSuccess) return LocalRedirect("/User/Teams/List");

                    return Page();
                }
                return new BadRequestResult();
            }

            return Page();
        }

        public void JoinedTeamSuccessfully(object sender, TeamCreatedArgs args)
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var profileId = _userInformation.GetUserProfileIdAsync(identityId).Result;

            _teamMiles.MilesPledged += MilesPledgedSuccess;
            _teamMiles.FailedToPledgeMiles += JoinFailed;

            _teamMiles.PledgeMiles(new PledgeMilesDto(args.Team.Id, profileId, Input.MilesPledged)).Wait();
        }
        public void AddToBoardParticipants(object sender, TeamCreatedArgs args)
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var profileId = _userInformation.GetUserProfileIdAsync(identityId).Result;

            int boardId = _msgBoardService.GetOrCreateTeamBoard(args.Team.Id).Result;
            _msgBoardService.AddParticipant(profileId, boardId).Wait();
        }

        public void MilesPledgedSuccess(object sender, MilesPledgedArgs args)
        {
            JoinedSuccess = true;
        }

        public void JoinFailed(object sender, Exception args)
        {
            JoinedSuccess = false;
            TempData[MessageKey] = args.Message;
        }
    }
}