using System.Collections.Generic;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;
using OWM.Domain.Entities.Enums;

namespace OWM.Application.Services.Interfaces
{
    public interface ITeamSearchService
    {
        Task<int> Count(string searchExpression, int occupation, AgeRange ageRange);
        Task<List<TeamInformationDto>> Search(SearchTeamDto search, int skip, int take);
    }
}