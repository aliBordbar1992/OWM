using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OWM.Domain.Entities.Enums;

namespace OWM.Domain.Entities
{
    public class Profile : BaseAuditClass
    {
        public ILazyLoader LazyLoader { get; }
        private User _identity;
        private Occupation _occupation;
        private Country _country;
        private City _city;
        private Ethnicity _ethnicity;
        private ICollection<Interest> _interests;
        private ICollection<MilesPledged> _milesPledged;
        private ICollection<TeamMember> _teams;

        public Profile()
        {
            Interests = new List<Interest>();
            MilesPledged = new List<MilesPledged>();
            Teams = new List<TeamMember>();
        }

        public Profile(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }

        public Occupation Occupation
        {
            get => LazyLoader.Load(this, ref _occupation);
            set => _occupation = value;
        }
        public Country Country
        {
            get => LazyLoader.Load(this, ref _country);
            set => _country = value;
        }
        public City City
        {
            get => LazyLoader.Load(this, ref _city);
            set => _city = value;
        }
        public Ethnicity Ethnicity
        {
            get => LazyLoader.Load(this, ref _ethnicity);
            set => _ethnicity = value;
        }
        public User Identity
        {
            get => LazyLoader.Load(this, ref _identity);
            set => _identity = value;
        }
        public ICollection<Interest> Interests
        {
            get => LazyLoader.Load(this, ref _interests);
            set => _interests = value;
        }
        public ICollection<MilesPledged> MilesPledged
        {
            get => LazyLoader.Load(this, ref _milesPledged);
            set => _milesPledged = value;
        }
        public ICollection<TeamMember> Teams
        {
            get => LazyLoader.Load(this, ref _teams);
            set => _teams = value;
        }
    }
}
