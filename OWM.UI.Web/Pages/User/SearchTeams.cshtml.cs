using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OWM.UI.Web.Pages.User
{
    [Authorize]
    public class SearchTeamsModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}