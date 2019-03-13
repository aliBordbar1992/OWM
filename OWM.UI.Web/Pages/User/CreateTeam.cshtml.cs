using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Application.Services.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OWM.Application.Services.EventHandlers;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles = "User")]
    public class CreateTeamModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;
        private readonly ITeamMilesService _teamMiles;
        private readonly IOccupationInformationService _ocpInformationService;
        public string AgeRange { get; set; }
        public List<SelectListItem> OccupationOptions;
        [BindProperty] public InputModel Input { get; set; }
        public const string MessageKey = nameof(MessageKey);
        public bool NoOccupation { get; set; }

        public class InputModel
        {
            public InputModel()
            {
                Occupations = new List<SelectedOccupation>();
            }
            [Required(ErrorMessage = "Team name is required.")]
            public string TeamName { get; set; }

            //[Required(ErrorMessage = "Pledged miles should not be empty.")]
            [AssertThat("MilesPledged > 0", ErrorMessage = "Miles pledged must be greater than 0.")]
            public float MilesPledged { get; set; }

            public List<SelectedOccupation> Occupations { get; set; }

            [AssertThat("OccupationFilter == true",
                ErrorMessage = "You can select occupations if you check 'Only some occupations can join this team' box")]
            public string SelectedOccupations { get; set; }
            public bool OccupationFilter { get; set; }

            [Required(ErrorMessage = "Provide a short description for your team")]
            public string Description { get; set; }
        }
        public class SelectedOccupation
        {
            public SelectedOccupation()
            {
            }
            public SelectedOccupation(string name, int value)
            {
                this.name = name;
                this.value = value;
            }

            public string name { get; set; }
            public int value { get; set; }
        }

        public CreateTeamModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
            , ITeamsManagerService teamManager
            , ITeamMilesService teamMiles
            , IOccupationInformationService ocpInformationService)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
            _teamMiles = teamMiles;
            _ocpInformationService = ocpInformationService;
            OccupationOptions = new List<SelectListItem>();
        }

        public async Task OnGetAsync()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);
            if (userInfo.OccupationOrder != 1)
            {
                var ocp = await _userInformation.GetUserOccupationAsync(identityId);
                FillOccupationDropdown(ocp.Id);
                NoOccupation = true;
            }

            var aR = AgeRangeCalculator.GetAgeRange(userInfo.DateOfBirth.Value);
            AgeRange = AgeRangeCalculator.GetAgeRangeCaption(aR);
        }
        public void FillOccupationDropdown(int ocpId)
        {
            OccupationOptions = _ocpInformationService.GetOccupations().Where(x => x.Id != ocpId && x.Order == 1).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }

        public async Task OnPostAsync()
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var userOcp = await _userInformation.GetUserOccupationAsync(identityId);
            FillOccupationDropdown(userOcp.Id);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);
            var aR = AgeRangeCalculator.GetAgeRange(userInfo.DateOfBirth.Value);
            AgeRange = AgeRangeCalculator.GetAgeRangeCaption(aR);

            if (userInfo.OccupationOrder != 1)
            {
                var ocp = await _userInformation.GetUserOccupationAsync(identityId);
                FillOccupationDropdown(ocp.Id);
                NoOccupation = true;
            }

            if (ModelState.IsValid)
            {
                
                if (Input.OccupationFilter)
                {
                    
                    if (!string.IsNullOrEmpty(Input.SelectedOccupations))
                    {
                        Input.Occupations =
                            JsonConvert.DeserializeObject<List<SelectedOccupation>>(Input.SelectedOccupations);
                    }

                    Input.Occupations.Add(new SelectedOccupation(userOcp.Name, userOcp.Id));

                    foreach (var ocp in Input.Occupations)
                    {
                        if (!await _ocpInformationService.AssertOccupationExists(ocp.value))
                        {
                            ModelState.AddModelError(string.Empty, $"Occupation {ocp.name} not found. Refine your selection.");
                            return;
                        }
                    }
                }

                var createTeamDto = await MapToDto(Input);

                _teamManager.TeamCreated += PledgeMilesToCreatedTeam;
                _teamManager.CreationFailed += CreateTeamFailed;

                await _teamManager.CreateTeam(createTeamDto);
            }
        }

        private async Task<CreateTeamDto> MapToDto(InputModel input)
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);
            var aR = AgeRangeCalculator.GetAgeRange(userInfo.DateOfBirth.Value);

            return new CreateTeamDto
            {
                Name = input.TeamName,
                OccupationFilter = input.OccupationFilter,
                Description = input.Description,
                OccupationIds = input.Occupations.Select(x => x.value).ToArray(),
                Range = aR,
                ProfileId = userInfo.ProfileId
            };
        }

        public void PledgeMilesToCreatedTeam(object sender, TeamCreatedArgs args)
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            int profileId = _userInformation.GetUserProfileIdAsync(identityId).Result;

            _teamMiles.MilesPledged += MilesPledgedSuccessfully;
            _teamMiles.FailedToPledgeMiles += CreateTeamFailed;

            _teamMiles.PledgeMiles(new PledgeMilesDto(args.Team.Id, profileId, Input.MilesPledged)).Wait();
        }
        public void MilesPledgedSuccessfully(object sender, MilesPledgedArgs args)
        {
            TempData[MessageKey] = "Team created successfully!";
        }

        public void CreateTeamFailed(object sender, Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
        }
    }
}