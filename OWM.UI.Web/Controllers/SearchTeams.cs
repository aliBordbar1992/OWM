using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using OWM.UI.Web.Enums;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    public partial class SearchTeams:Controller
    {
        public class SearchModel
        {
            public string SearchExpression { get; set; }
            public int MilesOrder { get; set; }
            public int MemberOrder { get; set; }
            public string Occupation { get; set; }
        }

        [HttpPost("/Teams/Search/")]
        public JsonResult Post(SearchModel search)
        {
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
