using System.Collections.Generic;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;

namespace OWM.Application.Services.Interfaces
{
    public interface ITeamMessageBoardService
    {
        Task<int> GetOrCreateTeamBoard(int teamId);
        Task<List<ParticipantInforationDto>> GetMessageBoardParticipants(int boardId);
        Task<List<MessageDto>> GetMessagesInBoard(int boardId);
        int GetUnreadBoards(int profileId);
        Task PostMessage(int profileId, int boardId, string text);
        List<TeamBoardsDto> GetAllTeamBoards(int profileId);
        void EnsureTeamsHaveBoard();
    }
}