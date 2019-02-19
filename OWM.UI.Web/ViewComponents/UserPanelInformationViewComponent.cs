using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.Dtos;
using OWM.UI.Web.Dtos;

namespace OWM.UI.Web.ViewComponents
{
    public class UserPanelInformationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var info = new UserInformationDto
            {
                UserImage = "",
                Name = "Gholi",
                Surname = "Test",
                Occupation ="None",
                TeamJoined = "23",
                MilesPledged ="765",
                MilesCompleted = "34",
                Ethnicity = "test"
            };

            return View("/Pages/User/Shared/Components/UserPanelInformation/Default.cshtml", info);
        }
    }
}