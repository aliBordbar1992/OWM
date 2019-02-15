using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    public class SearchTeams:Controller
    {
        [HttpGet("/Teams/SearchTeams")]
        public JsonResult Get()
        {
            if (!CheckUserLogin.IsAuthenticated())
            {
                return null;
            }
            var searchedTeamlist = new List<Teams> {new Teams()
            {
                TeamName = "London Foundation",
                Occupation = "Europian",
                Members = 5,
                MilesPledged = 25,
                MilesCompeleted = 17,
                Description = "Some quick example text to build on the card title and make up the bulk of the cards content.",
                DateCreated = FullDateTime("15/01/2016")
            }};
            return new JsonResult(searchedTeamlist);
        }

        public string FullDateTime(string date)
        {
            var dt = date.Split("/");
            return new DateTime(Convert.ToInt32(dt[2]), Convert.ToInt32(dt[1]), Convert.ToInt32(dt[0]))
                .ToString("dd MMMM yyyy", CultureInfo.InvariantCulture);
        }
        public class Teams
        {
            public string TeamName { get; set; }
            public int Members { get; set; }
            public int MilesPledged { get; set; }
            public int MilesCompeleted { get; set; }
            public int OccupationId { get; set; }
            public string Occupation { get; set; }
            public int AgeRange { get; set; }
            public string Description { get; set; }
            public string DateCreated { get; set; }
        }
    }
}
