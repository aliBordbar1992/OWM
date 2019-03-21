using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OWM.Domain.Entities
{
    public class Message : BaseAuditClass
    {
        private Message _replyToMessage;
        private MessageBoard _board;
        private Participant _from;
        public ILazyLoader LazyLoader { get; }

        public Message()
        {
        }
        public Message(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public Message ReplyToMessage
        {
            get => LazyLoader.Load(this, ref _replyToMessage);
            set => _replyToMessage = value;
        }

        public MessageBoard Board
        {
            get => LazyLoader.Load(this, ref _board);
            set => _board = value;
        }

        public Participant From
        {
            get => LazyLoader.Load(this, ref _from);
            set => _from = value;
        }

        public string Text { get; set; }
    }
}