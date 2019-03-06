using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Email;
using OWM.Application.Services.EventHandlers;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles="User")]
    public class InviteMemberModel : PageModel
    {
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IUserInformationService _userInformation;
        private readonly ITeamsManagerService _teamManager;
        private ITeamInvitationsService _invitations;

        public InviteMemberModel(IServiceProvider serviceProvider
            , UserManager<Domain.Entities.User> userManager
            , SignInManager<Domain.Entities.User> signInManager)
        {
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _signInManager = signInManager;

            _userInformation = serviceProvider.GetRequiredService<IUserInformationService>();
            _teamManager = serviceProvider.GetRequiredService<ITeamsManagerService>();
            _invitations = serviceProvider.GetRequiredService<ITeamInvitationsService>();
        }

        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string Name { get; set; }
        public const string MessageKey = nameof(MessageKey);

        [BindProperty] public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Email address can not be empty")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress]
            public string EmailAddress { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Enter recipient name")]
            public string RecipientName { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Write an invite message")]
            public string Message { get; set; }
        }



        public async Task<IActionResult> OnGetAsync(int? teamid)
        {
            if (!teamid.HasValue)
            {
                return LocalRedirect("/User/Teams/List");
            }

            TeamId = teamid.Value;

            string identityId = _signInManager.UserManager.GetUserId(User);
            var userInfo = await _userInformation.GetUserProfileInformationAsync(identityId);

            if (!await _teamManager.IsMemberOfTeam(TeamId, userInfo.ProfileId))
                return NotFound();

            var t = await _teamManager.GetTeamInviteInformation(TeamId);
            TeamName = t.TeamName;

            Name = $"{userInfo.Name} {userInfo.Surname}";

            Input = new InputModel
            {
                Message = @"Hi! I'm " + Name + ".\nJoin us in our team in OWM,\nit's so much fun!"
            };

            return Page();
        }


        public async Task OnPostAsync(int teamid)
        {
            if (ModelState.IsValid)
            {
                var invitedUser = await _userManager.FindByEmailAsync(Input.EmailAddress);
                if (invitedUser == null)
                {
                    _invitations.InvitationAdded += SendInvitationEmail;

                    await _invitations.AddInvitation(await MapToDto(teamid, null));
                }
                else
                {
                    int invitedUserProfileId = await _userInformation.GetUserProfileIdAsync(invitedUser.Id);
                    if (await _teamManager.IsMemberOfTeam(teamid, invitedUserProfileId))
                    {
                        ModelState.AddModelError(string.Empty, $"A member with email {Input.EmailAddress} is already joined this team.");
                        return;
                    }

                    _invitations.InvitationAdded += SendInvitationEmail;
                    await _invitations.AddInvitation(await MapToDto(teamid, invitedUserProfileId));
                }

                TempData[MessageKey] = "Invitation email sent successfully.";
            }
        }

        private async Task<InvitationInformationDto> MapToDto(int teamId, int? recipientId)
        {
            var loggedInUser = await _signInManager.UserManager.GetUserAsync(User);
            var loggedInUserInfo = await _userInformation.GetUserProfileInformationAsync(loggedInUser.Id);

            var teamInfo = await _teamManager.GetTeamInviteInformation(teamId);

            var dto = new InvitationInformationDto
            {
                TeamId = teamId,
                EmailAddress = Input.EmailAddress,
                Message = Input.Message,
                RecipientName = Input.RecipientName,
                TeamName = teamInfo.TeamName,
                TeamGuid = teamInfo.TeamGuid,
                RecipientId = recipientId,
                SenderFullName = $"{loggedInUserInfo.Name} {loggedInUserInfo.Surname}",
                SenderId = loggedInUserInfo.ProfileId
            };

            return dto;
        }

        public void SendInvitationEmail(object sender, AddInvitationArgs args)
        {
            var callbackUrl = Url.Page(
                "/TeamInvite",
                pageHandler: null,
                values: new { id = args.Info.Token },
                protocol: Request.Scheme);
            string encodedUrl = HtmlEncoder.Default.Encode(callbackUrl);
            var hostingEnv = _serviceProvider.GetRequiredService<IHostingEnvironment>();

            TeamInvitationEmailSender emailSender =
                new TeamInvitationEmailSender(hostingEnv
                    , args.Info.TeamName
                    , args.Info.SenderFullName
                    , args.Info.RecipientName
                    , args.Info.EmailAddress
                    , args.Info.Message
                    , encodedUrl);
            emailSender.Send();
        }
    }
}