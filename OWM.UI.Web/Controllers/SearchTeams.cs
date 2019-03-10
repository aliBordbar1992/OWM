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
        public SearchModel Post([FromBody] SearchModel search)
        {
            return search;
        }
    }
}
