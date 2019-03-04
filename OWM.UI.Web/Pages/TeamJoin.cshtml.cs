using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OWM.UI.Web.Pages
{
    public class TeamJoinModel : PageModel
    {
        public bool IsJoin { get; set; }
        public bool IsInvite { get; set; }
        public string TeamName { get; set; }
        public void OnGetJoin(int? id)
        {
            IsJoin = true;
            IsInvite = false;
        }

        public void OnGetInvite(string id)
        {
            IsJoin = false;
            IsInvite = true;
        }
    }
}