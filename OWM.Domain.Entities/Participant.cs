using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OWM.Domain.Entities
{
    public class Participant : BaseAuditClass
    {
        private Profile _profile;
        private MessageBoard _board;
        public ILazyLoader LazyLoader { get; }

        public Participant()
        {
        }
        public Participant(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public Profile Profile
        {
            get => LazyLoader.Load(this, ref _profile);
            set => _profile = value;
        }
        public MessageBoard Board
        {
            get => LazyLoader.Load(this, ref _board);
            set => _board = value;
        }

        public DateTime LastReadTimeStamp { get; set; }
    }
}