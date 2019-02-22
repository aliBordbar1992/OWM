using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OWM.Application.Services;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class exploreModel : PageModel
    {
        private readonly IOccupationInformationService _ocpInformation;
        public List<SelectListItem> OccupationOptions;

        public exploreModel(IOccupationInformationService ocpInformation)
        {
            _ocpInformation = ocpInformation;
            OccupationOptions = new List<SelectListItem>();
        }

        public void OnGet()
        {
            FillOccupationDropdown();
        }
        public void FillOccupationDropdown()
        {
            OccupationOptions = _ocpInformation.GetOccupations().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id + ""
            }).ToList().Result;
        }
    }
}