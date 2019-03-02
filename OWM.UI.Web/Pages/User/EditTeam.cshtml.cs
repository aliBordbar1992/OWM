using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OWM.UI.Web.Pages.User
{
    public class EditTeamModel : PageModel
    {
        public int TeamId { get; set; }

        public async Task<IActionResult> OnGet(int? teamid)
        {
            if (!teamid.HasValue)
            {
                return LocalRedirect("/User/Teams/List");
            }

            TeamId = teamid.Value;
            return Page();
        }
    }
}