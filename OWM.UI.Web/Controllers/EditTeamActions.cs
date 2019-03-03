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

        [HttpGet("/api/kickmember")]
        public async Task<IActionResult> KickMember(int tId , int pId ,int mpId)
        {
            try
            {
                int success = await _teamManager.KickMember(tId , pId , mpId);
                string displayMessage = GetKickMemberMessage(success);
                return Json(new ApiResponse
                {
                    Success = success == -1,
                    DisplayMessage = displayMessage,
                    ErrorCode = success
                });
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("An error occurred during processing your request. Try again.");
            }
        }

        private string GetKickMemberMessage(int success)
        {
            switch (success)
            {
                case -1:
                    return "Team not found";
                case -2:
                    return "Member not found";
                case -3:
                    return "Something happened, try again.";
                default:
                    return "Member kick out from team";
            }
        }

        [HttpGet("/api/SaveChanges")]
        public async Task<IActionResult> SaveChanges(int tId, string description)
        {
            try
            {
                int success = await _teamManager.UpdateDescription(tId, description);
                return Json(new ApiResponse
                {
                    Success = success == 0,
                    DisplayMessage =
                        success == 0
                            ? "Update successfull!"
                            : "Something happened, try again.",
                    ErrorCode = success
                });
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("An error occurred during processing your request. Try again.");
            }
        }
    }
}
