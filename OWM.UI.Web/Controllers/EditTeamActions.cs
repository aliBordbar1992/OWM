using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.EventHandlers;
using OWM.Application.Services.Exceptions;
using OWM.Application.Services.Interfaces;
using OWM.UI.Web.Dtos;

namespace OWM.UI.Web.Controllers
{
    [Route("api")]
    [Authorize(Roles = "User")]
    public class EditTeamActions:Controller
    {
        private readonly ITeamsManagerService _teamManager;
        private readonly ITeamMilesService _teamMiles;


        public EditTeamActions(ITeamsManagerService teamManager
        , ITeamMilesService teamMiles)
        {
            _teamManager = teamManager;
            _teamMiles = teamMiles;
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

        [HttpGet("/api/unblockMember")]
        public async Task<IActionResult> UnBlockMember(int tId , int pId ,int mpId)
        {
            try
            {
                int success = await _teamManager.UnKickMember(tId , pId , mpId);
                string displayMessage = GetUnKickMemberMessage(success);
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

        private string GetUnKickMemberMessage(int success)
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
                    return "Member unblocked successfully";
            }
        }

        [HttpGet("/api/blockMember")]
        public async Task<IActionResult> BlockMember(int tId , int pId ,int mpId)
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
                    return "Member blocked successfully";
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



        private ApiResponse _pledgedMilesResponse;
        [HttpGet("/api/Miles/IncreaseMilesPledged")]
        public async Task<IActionResult> IncreaseMilesPledged(int tId, int pId, float miles)
        {
            try
            {
                _teamMiles.PledgedMilesUpdated += PledgeMilesUpdated;
                _teamMiles.FailedToPledgeMiles += PledgeMilesUpdateFailed;

                await _teamMiles.IncreasePledgedMilesBy(tId, pId, miles);

                return Json(_pledgedMilesResponse);
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("An error occurred during processing your request. Try again.");
            }
        }
        public void PledgeMilesUpdated(object sender, MilesPledgedArgs args)
        {
            _pledgedMilesResponse = new ApiResponse
            {
                Success = true,
                ErrorCode = 0,
                DisplayMessage = "Pledged miles increased successfully",
                Data = _teamMiles.GetTeamMilesInformation(args.MilesPledged.Team.Id, args.MilesPledged.Profile.Id)
                    .Result
            };
        }
        public void PledgeMilesUpdateFailed(object sender, Exception args)
        {
            _pledgedMilesResponse = new ApiResponse
            {
                Success = false,
                ErrorCode = -1,
                DisplayMessage = "Failed to increase miles pledged",
                Data = args.Message
            };
        }


        private ApiResponse _completeMilesResponse;
        private int _teamId;
        private int _profileId;
        [HttpGet("/api/Miles/CompleteMiles")]
        public async Task<IActionResult> CompleteMiles(int tId, int pId, float miles)
        {
            _teamId = tId;
            _profileId = pId;
            try
            {
                if (await _teamMiles.CanCompleteMiles(tId, pId))
                {
                    _teamMiles.PledgedMilesUpdated += CompleteMilesUpdated;
                    _teamMiles.FailedToPledgeMiles += CompleteMilesUpdateFailed;

                    await _teamMiles.IncreaseMilesCompletedBy(tId, pId, miles);

                    return Json(_completeMilesResponse);
                }
                else
                {
                    return Json(new ApiResponse
                    {
                        Success = false,
                        ErrorCode = -1,
                        DisplayMessage = "Failed to add miles completed",
                        Data = "You cannot complete miles"
                    });
                }
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("An error occurred during processing your request. Try again.");
            }
        }
        public void CompleteMilesUpdated(object sender, MilesPledgedArgs args)
        {
            _completeMilesResponse = new ApiResponse
            {
                Success = true,
                ErrorCode = 0,
                DisplayMessage = "Complete miles increased successfully",
                Data = _teamMiles.GetTeamMilesInformation(args.MilesPledged.Team.Id, args.MilesPledged.Profile.Id)
                    .Result
            };
        }
        public void CompleteMilesUpdateFailed(object sender, Exception args)
        {
            var t = args.GetType();
            if (t == typeof(CompleteMilesFailedException))
            {
                _completeMilesResponse = new ApiResponse
                {
                    Success = false,
                    ErrorCode = -1,
                    DisplayMessage = args.Message,
                    Data = _teamMiles.GetTeamMilesInformation(_teamId, _profileId)
                        .Result
                };
            }
            else
            {
                _completeMilesResponse = new ApiResponse
                {
                    Success = false,
                    ErrorCode = -1,
                    DisplayMessage = $"Failed to add miles completed.",
                    Data = _teamMiles.GetTeamMilesInformation(_teamId, _profileId)
                        .Result
                };
            }
           
        }
    }
}
