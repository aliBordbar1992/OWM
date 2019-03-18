using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OWM.Domain.Entities
{
    public class MessageBoard : BaseAuditClass
    {
        public ILazyLoader LazyLoader { get; }
        private Team _forTeam;
        private ICollection<Message> _messages;
        private ICollection<Participant> _participants;

        public MessageBoard()
        {
            Messages= new List<Message>();
            Participants = new List<Participant>();
        }
        public MessageBoard(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }


        public Team ForTeam
        {
            get => LazyLoader.Load(this, ref _forTeam);
            set => _forTeam = value;
        }
        public ICollection<Message> Messages
        {
            get => LazyLoader.Load(this, ref _messages);
            set => _messages = value;
        }
        public ICollection<Participant> Participants
        {
            get => LazyLoader.Load(this, ref _participants);
            set => _participants = value;
        }
    }
}