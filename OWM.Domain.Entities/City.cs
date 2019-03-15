using Microsoft.EntityFrameworkCore.Infrastructure;
using URF.Core.EF.Trackable;

namespace OWM.Domain.Entities
{
    public class City : Entity
    {
        private Country _country;
        public ILazyLoader LazyLoader { get; }

        public City()
        {
        }

        public City(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public int Id { get; set; }
        public int CustomCityId { get; set; }

        public Country Country
        {
            get => LazyLoader.Load(this, ref _country);
            set => _country = value;
        }

        public string Name { get; set; }
    }
}