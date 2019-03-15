using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OWM.Domain.Entities.Enums;

namespace OWM.Domain.Entities
{
    public class Team : BaseAuditClass
    {
        private ICollection<MilesPledged> _pledgedMiles;
        private ICollection<TeamOccupations> _allowedOccupations;
        private ICollection<TeamMember> _members;
        private MessageBoard _board;
        public ILazyLoader LazyLoader { get; }

        public Team()
        {
            PledgedMiles = new List<MilesPledged>();
            AllowedOccupations = new List<TeamOccupations>();
            Members = new List<TeamMember>();
        }

        public Team(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public bool OccupationFilter { get; set; }
        public bool IsClosed { get; set; }
        public Guid Identity { get; set; }
        public AgeRange AgeRange { get; set; }

        //public MessageBoard Board
        //{
        //    get => LazyLoader.Load(this, ref _board);
        //    set => _board = value;
        //}

        public ICollection<MilesPledged> PledgedMiles
        {
            get => LazyLoader.Load(this, ref _pledgedMiles);
            set => _pledgedMiles = value;
        }
        public ICollection<TeamOccupations> AllowedOccupations
        {
            get => LazyLoader.Load(this, ref _allowedOccupations);
            set => _allowedOccupations = value;
        }
        public ICollection<TeamMember> Members
        {
            get => LazyLoader.Load(this, ref _members);
            set => _members = value;
        }
    }
}