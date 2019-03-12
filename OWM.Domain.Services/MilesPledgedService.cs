using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class MilesPledgedService : Service<MilesPledged>, IMilesPledgedService
    {
        private readonly DbContext _context;

        public MilesPledgedService(ITrackableRepository<MilesPledged> repository, DbContext context) : base(repository)
        {
            _context = context;
        }

        public IEnumerable<MilesPledged> GetRecentMilePledges(int take)
        {
            return Repository.Queryable()
                .Include(x => x.Profile)
                .ThenInclude(x => x.City)
                .Include(x => x.Profile)
                .ThenInclude(x => x.Country)
                .OrderByDescending(x => x.Created)
                .Take(take)
                .AsEnumerable();
        }

        public float GetTotalMilesPledged()
        {
            return Repository.Queryable()
                .Sum(x => x.Miles);
        }
    }
}
