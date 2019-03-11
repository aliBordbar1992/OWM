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
    public class SearchTeams:Controller
    {
        private readonly ITeamSearchService _search;

        public SearchTeams(ITeamSearchService search)
        {
            _search = search;
        }

        [HttpPost("/api/searchteams")]
        public JsonResult SearchTeamsDto([FromBody] SearchTeamDto search)
        {
            int total = _search.Count(search.SearchExpression, search.Occupation, search.AgeRange).Result;
            var searchedTeamList = _search.Search(search, 0, 10).Result;
            return new JsonResult(searchedTeamList);
        }
    }
}
