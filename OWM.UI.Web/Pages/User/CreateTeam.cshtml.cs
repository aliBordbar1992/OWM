using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OWM.Application.Services;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Application.Services.Utils;
using OWM.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles = "User")]
    public class CreateTeamModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManager _teamManager;
        private readonly IOccupationInformationService _ocpInformationService;
        public List<SelectListItem> OccupationOptions;
        [BindProperty] public InputModel Input { get; set; }
        public List<SelectListItem> AgeRanges => Application.Services.Extensions.SelectList.Of<AgeRange>().ToList();
        public string AgeRange { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Team name is required.")]
            public string TeamName { get; set; }

            [Required(ErrorMessage = "Pledged miles should not be empty.")]
            public int? MilesPledged { get; set; }

            public int[] OccupationId => Array.ConvertAll(SelectedOccupations.Split(',').ToArray(), int.Parse);

            [AssertThat("OccupationFilter == true",
                ErrorMessage = "You can select occupations if you check 'Only some occupations can join this team' box")]
            public string SelectedOccupations { get; set; }
            public bool OccupationFilter { get; set; }

            [Required(ErrorMessage = "Provide a short description for your team")]
            public string Description { get; set; }
        }


        public CreateTeamModel(SignInManager<Domain.Entities.User> signInManager
            , IUserInformationService userInformation
            , ITeamsManager teamManager
            , IOccupationInformationService ocpInformationService)
        {
            _signInManager = signInManager;
            _userInformation = userInformation;
            _teamManager = teamManager;
            _ocpInformationService = ocpInformationService;
            OccupationOptions = new List<SelectListItem>();
        }

        public void OnGet()
        {
            FillOccupationDropdown();
        }
        public void FillOccupationDropdown()
        {
            OccupationOptions = _ocpInformationService.GetOccupations().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var createTeamDto = MapToDto(Input);
                _teamManager.CreateTeam(createTeamDto);
            }

            return Page();
        }


        private CreateTeamDto MapToDto(InputModel input)
        {
            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = _userInformation.GetUserProfile(identityId);
            var aR = AgeRangeCalculator.GetAgeRange(userInfo.DateOfBirth.Value);

            return new CreateTeamDto
            {
                Name = input.TeamName,
                OccupationFilter = input.OccupationFilter,
                Description = input.Description,

            };
        }
    }
}