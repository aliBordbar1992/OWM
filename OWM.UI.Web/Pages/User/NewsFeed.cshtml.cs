using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OWM.UI.Web.Pages.User
{
    [Authorize]
    public class NewsFeedModel : PageModel
    {
        public void OnGet()
        {
           
        }
    }
}