using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class Occupation : Entity
    {
        private ICollection<TeamOccupations> _inTeams;
        public ILazyLoader LazyLoader { get; }

        public Occupation()
        {
            InTeams = new List<TeamOccupations>();
        }

        public Occupation(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public ICollection<TeamOccupations> InTeams
        {
            get => LazyLoader.Load(this, ref _inTeams);
            set => _inTeams = value;
        }
    }
}