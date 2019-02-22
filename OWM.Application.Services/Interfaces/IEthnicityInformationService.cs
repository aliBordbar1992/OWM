using System.Collections.Generic;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Interfaces
{
    public interface IEthnicityInformationService
    {
        IAsyncEnumerable<Ethnicity> GetEthnicities();
    }
}