using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace OWM.Domain.Services
{
    public class MessageBoardService : Service<MessageBoard>, IMessageBoardService
    {
        public MessageBoardService(ITrackableRepository<MessageBoard> repository) : base(repository)
        {
        }
    }
}
