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

        [HttpPost("/api/Teams/Search")]
        public JsonResult SearchTeamsList([FromBody] SearchTeamDto search)
        {
            var searchedTeamList = _search.Search(search, search.Skip, search.Take).Result;
            return new JsonResult(searchedTeamList);
        }

        [HttpPost("/api/Search/Count")]
        public int SearchTeamsCount([FromBody] SearchTeamDto search)
        {
            return  _search.Count(search.SearchExpression, search.Occupation, search.AgeRange).Result;
        }
    }
}
