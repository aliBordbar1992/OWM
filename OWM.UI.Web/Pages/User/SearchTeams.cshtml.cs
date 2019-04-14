using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OWM.UI.Web.Pages.User
{
    [Authorize(Roles = "User")]
    public class SearchTeamsModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}