using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class UserProfileModel : PageModel
    {
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;

        public UserProfileModel(IUserInformationService userInformation, ITeamsManagerService teamManager)
        {
            _userInformation = userInformation;
            _teamManager = teamManager;
        }

        public int ProfileId { get; set; }
        [BindProperty] public ProfileInformationDto MemberInformation { get; set; }

        public async Task<IActionResult> OnGet(int? profileid)
        {
            if (!profileid.HasValue)
            {
                return LocalRedirect("/Explore");
            }

            ProfileId = profileid.Value;
            MemberInformation = await _teamManager.GetTeamMemberProfileInformation(ProfileId);

            return Page();
        }
    }
}