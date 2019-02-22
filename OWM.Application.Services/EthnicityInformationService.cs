using System.Collections.Generic;
using System.Linq;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;

namespace OWM.Application.Services
{
    public class EthnicityInformationService : IEthnicityInformationService
    {
        private readonly IEthnicityService _ethnicityService;

        public EthnicityInformationService(IEthnicityService ethnicityService)
        {
            _ethnicityService = ethnicityService;
        }

        public IAsyncEnumerable<Ethnicity> GetEthnicities() => _ethnicityService.Queryable().ToAsyncEnumerable();
    }
}