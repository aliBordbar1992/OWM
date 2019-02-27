using System.Collections.Generic;
using System.Threading.Tasks;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Interfaces
{
    public interface IOccupationInformationService
    {
        IAsyncEnumerable<Occupation> GetOccupations();
        Task<bool> AssertOccupationExists(int occupationId);
    }
}