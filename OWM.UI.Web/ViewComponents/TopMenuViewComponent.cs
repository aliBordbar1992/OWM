using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OWM.UI.Web.Dtos;

namespace OWM.UI.Web.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new TopMenuDto {IsSignedIn = true, Name = "Gholi"};
            return View(model);
        }
    }
}