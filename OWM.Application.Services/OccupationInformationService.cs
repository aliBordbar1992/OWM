using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;

namespace OWM.Application.Services
{
    public class OccupationInformationService : IOccupationInformationService
    {
        private readonly IOccupationService _occupationService;

        public OccupationInformationService(IOccupationService occupationService)
        {
            _occupationService = occupationService;
        }

        public IAsyncEnumerable<Occupation> GetOccupations() => _occupationService.Queryable().OrderBy(x => x.Order).ToAsyncEnumerable();
        public async Task<bool> AssertOccupationExists(int occupationId)
        {
            return await _occupationService.ExistsAsync(occupationId);
        }
    }
}