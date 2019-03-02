using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace OWM.UI.Web.Controllers
{
    [Route("api")]
    public class EditTeamActions:Controller
    {
        [HttpGet("/api/Prevent")]
        public JsonResult ChangePrevent(bool check)
        {
            return new JsonResult("Change Prevent");
        }

        [HttpGet("/api/SaveChanges")]
        public JsonResult SaveChanges(string description)
        {
            return new JsonResult("Save Changes");
        }
    }
}
