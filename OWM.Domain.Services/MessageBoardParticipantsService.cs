using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class MessageBoardParticipantsService : Service<Participant>, IMessageBoardParticipantsService
    {
        public MessageBoardParticipantsService(ITrackableRepository<Participant> repository) : base(repository)
        {
        }
    }
}