using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OWM.Application.Services.Interfaces;
using OWM.UI.Web.Dtos;

namespace OWM.UI.Web.Controllers
{
    [Route("api")]
    [Authorize(Roles = "User")]
    public class EditTeamActions:Controller
    {
        private readonly ITeamsManagerService _teamManager;

        public EditTeamActions(ITeamsManagerService teamManager)
        {
            _teamManager = teamManager;
        }

        [HttpGet("/api/Prevent")]
        public async Task<IActionResult> ChangePrevent(int tId, bool open)
        {
            try
            {
                int success = await _teamManager.CloseTeam(tId, open);
                return Json(new ApiResponse
                {
                    Success = success == 0,
                    DisplayMessage =
                        success == 0
                            ? open ? "Members can join this team." : "Team is closed now."
                            : "Something happened, try again.",
                    ErrorCode = success
                });
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("An error occurred during processing your request. Try again.");
            }
        }

        [HttpGet("/api/SaveChanges")]
        public JsonResult SaveChanges(int tId, string description)
        {
            return new JsonResult("Save Changes");
        }
    }
}
