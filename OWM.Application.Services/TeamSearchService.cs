using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Enums;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities.Enums;
using OWM.Domain.Services.Interfaces;

namespace OWM.Application.Services
{
    public class TeamSearchService : ITeamSearchService
    {
        private readonly ITeamService _teamService;
        private readonly IMilesPledgedService _milesPledgedService;

        public TeamSearchService(ITeamService teamService, IMilesPledgedService milesPledgedService)
        {
            _teamService = teamService;
            _milesPledgedService = milesPledgedService;
        }


        public async Task<int> Count(string searchExpression, int occupation, AgeRange ageRange)
        {
            var query = _teamService.Queryable().Where(x => x.Name.Contains(searchExpression));
            query = occupation == -1
                ? query
                : query.Where(x => x.AllowedOccupations.Any(o => o.OccupationId == occupation));
            query = ageRange == AgeRange.All ? query : query.Where(x => x.AgeRange == ageRange);

            return await query.AsQueryable().CountAsync();
        }

        public async Task<List<TeamInformationDto>> Search(SearchTeamDto search, int skip, int take)
        {
            var query = _teamService.Queryable()
                .Where(x => x.Name.Contains(search.SearchExpression));

            query = search.Occupation == -1
                ? query
                : query.Where(x => x.AllowedOccupations.Any(o => o.OccupationId == search.Occupation));

            query = search.AgeRange == AgeRange.All ? query : query.Where(x => x.AgeRange == search.AgeRange);

            if (search.MemberOrderBy != EnumOrderBy.Default)
            {
                query = search.MemberOrderBy == EnumOrderBy.Asc
                    ? query.OrderBy(x => x.Members.Count)
                    : query.OrderByDescending(x => x.Members.Count);
            }

            if (search.MilesOrderBy != EnumOrderBy.Default)
            {
                query = search.MilesOrderBy == EnumOrderBy.Asc
                    ? query.OrderBy(x => x.PledgedMiles.Sum(p => p.Miles))
                    : query.OrderByDescending(x => x.PledgedMiles.Sum(p => p.Miles));
            }

            var result = await query.Select(team => new TeamInformationDto
            {
                TeamId = team.Id,
                TeamName = team.Name,
                DateCreated = team.Created,
                Description = team.ShortDescription,
                IsClosed = team.IsClosed,
                Occupations = team.OccupationFilter
                    ? team.AllowedOccupations.Select(o => o.Occupation.Name).ToList()
                    : new List<string> {"All"},
                TotalMilesCompleted = team.PledgedMiles.SelectMany(p => p.CompletedMiles).Sum(c => c.Miles),
                TotalMilesPledged = team.PledgedMiles.Sum(p => p.Miles),
                TeamMembersCount = team.Members.Count,
            }).Skip(skip).Take(take).ToListAsync();

            return result;
        }
    }
}