using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities.Enums;
using OWM.UI.Web.Enums;
using EnumOrderBy = OWM.Application.Services.Enums.EnumOrderBy;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    public partial class SearchTeams:Controller
    {
        private readonly ITeamSearchService _search;

        public SearchTeams(ITeamSearchService search)
        {
            _search = search;
        }

        [HttpPost("/Teams/Search/")]
        public SearchModel Post([FromBody] SearchModel search)
        {
            int total = _search.Count(search.SearchExpression, search.Occupation, search.AgeRange).Result;

            var searchedTeamList = _search.Search(search, skip, 10).Result;
            return new JsonResult(searchedTeamList);
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

        //public ActionResult Search(int page = 1, string term = "", SortBy sortBy = SortBy.AddDate, SortOrder sortOrder = SortOrder.Desc)

    }
}
