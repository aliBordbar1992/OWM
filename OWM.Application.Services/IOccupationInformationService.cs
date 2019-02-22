using System.Collections.Generic;
using OWM.Domain.Entities;

namespace OWM.Application.Services
{
    public interface IOccupationInformationService
    {
        IAsyncEnumerable<Occupation> GetOccupations();
    }
}