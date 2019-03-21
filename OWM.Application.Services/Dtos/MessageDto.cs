using System;

namespace OWM.Application.Services.Dtos
{
    public class MessageDto
    {
        public ParticipantInforationDto Sender { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}