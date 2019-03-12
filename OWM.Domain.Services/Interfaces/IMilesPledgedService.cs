using System.Collections.Generic;
using System.Threading.Tasks;
using OWM.Domain.Entities;
using URF.Core.Abstractions.Services;

namespace OWM.Domain.Services.Interfaces
{
    public interface IMilesPledgedService : IService<MilesPledged>
    {
        IEnumerable<MilesPledged> GetRecentMilePledges(int take);
        float GetTotalMilesPledged();
    }
}