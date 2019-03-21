using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Exceptions;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class TeamMessageBoardService : ITeamMessageBoardService
    {
        private readonly ITeamService _teamService;
        private readonly IProfileService _profileService;
        private readonly IMessageBoardParticipantsService _participantsService;
        private readonly IMessageBoardService _msgBoardService;
        private readonly IUnitOfWork _unitOfWork;

        public TeamMessageBoardService(ITeamService teamService
            , IProfileService profileService
            , IMessageBoardParticipantsService participantsService
            , IMessageBoardService messageBoardService
            , IUnitOfWork unitOfWork)
        {
            _teamService = teamService;
            _profileService = profileService;
            _participantsService = participantsService;
            _msgBoardService = messageBoardService;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetOrCreateTeamBoard(int teamId)
        {
            var team = await _teamService.Queryable()
                .FirstOrDefaultAsync(x => x.Id == teamId);
            if (team == null)
                throw new TeamNotFoundException<int>($"No team found for id {teamId}", null, teamId);

            if (team.Board != null) return team.Board.Id;

            try
            {
                var board = new MessageBoard
                {
                    ForTeam = team,
                    Participants = CreateBoardParticipants(team.Members),
                    TrackingState = TrackingState.Added
                };

                _msgBoardService.ApplyChanges(board);
                await _unitOfWork.SaveChangesAsync();

                return board.Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public async Task<List<ParticipantInforationDto>> GetMessageBoardParticipants(int boardId)
        {
            var board = await _msgBoardService.Queryable()
                .FirstOrDefaultAsync(x => x.Id == boardId);
            if (board == null) throw new ArgumentException("No board found for the given id");

            var participants = board.Participants.Select(x => new ParticipantInforationDto
            {
                ProfilePicture = x.Profile.ProfileImageUrl,
                ProfileId = x.Profile.Id,
                FullName = x.Profile.Name + " " + x.Profile.Surname
            }).ToList();

            return participants;
        }

        public async Task<List<MessageDto>> GetMessagesInBoard(int boardId)
        {
            var board = await _msgBoardService.Queryable()
                .FirstOrDefaultAsync(x => x.Id == boardId);

            if (board == null)
                throw new ArgumentException("No board found for the given id");

            var result = board.Messages.Select(x => new MessageDto
            {
                Sender = new ParticipantInforationDto()
                {
                    ProfilePicture = x.From.Profile.ProfileImageUrl,
                    ProfileId = x.From.Profile.Id,
                    FullName = x.From.Profile.Name + " " + x.From.Profile.Surname
                },
                Text = x.Text,
                Date = x.Created
            }).ToList();

            return result;
        }

        public int GetUnreadBoards(int profileId)
        {
            return _participantsService.Queryable()
                .Where(x => x.Profile.Id == profileId)
                .Count(x => x.LastReadTimeStamp < x.Board.Modified);
        }

        public async Task PostMessage(int profileId, int boardId, string text)
        {
            var board = await _msgBoardService.Queryable().FirstOrDefaultAsync(x => x.Id == boardId);
            if (board == null) throw new ArgumentException("No board found for the given id");

            var participant = await _participantsService.Queryable().FirstOrDefaultAsync(x => x.Profile.Id == profileId);
            if (participant == null) throw new ArgumentException("No participant found for the given id");

            var newMsg = new Message
            {
                From = participant,
                Board = board,
                ReplyToMessage = null,
                Text = text,
                TrackingState = TrackingState.Added
            };

            board.TrackingState = TrackingState.Modified;
            board.Messages.Add(newMsg);

            _msgBoardService.ApplyChanges(board);
            await _unitOfWork.SaveChangesAsync();
        }

        public List<TeamBoardsDto> GetAllTeamBoards(int profileId)
        {
            if (!_teamService.Queryable().Any(x => x.Members.Any(m => m.ProfileId == profileId)))
                return new List<TeamBoardsDto>();

            return _msgBoardService.Queryable()
                .Where(x => x.Participants.Any(m => m.Profile.Id == profileId))
                .Select(x => new TeamBoardsDto
                {
                    TeamName = x.ForTeam.Name,
                    BoardId = x.Id,
                    Selected = false,
                    UnreadMessages = x.Messages.Count(msg => msg.Created > x.Participants.First(p => p.Profile.Id == profileId).LastReadTimeStamp)
                }).ToList();
        }

        public void EnsureTeamsHaveBoard()
        {
            var teams = _teamService.Queryable()
                .Where(x => x.Board == null).ToList();

            foreach (var team in teams)
            {
                int bId = GetOrCreateTeamBoard(team.Id).Result;
            }
        }

        public async Task UpdateParticipantReadCheck(int profileId, int boardId)
        {
            var participant = await _participantsService.Queryable().FirstOrDefaultAsync(x => x.Profile.Id == profileId && x.Board.Id == boardId);
            if (participant == null) throw new ArgumentException("No participant found for the given id");

            participant.LastReadTimeStamp = DateTime.Now;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> HasUnreadMessage(int profileId)
        {
            return await _participantsService.Queryable()
                .AnyAsync(x => x.Profile.Id == profileId && x.Board.Modified > x.LastReadTimeStamp);
        }

        public async Task AddParticipant(int profileId, int boardId)
        {
            var board = await _msgBoardService.Queryable().FirstOrDefaultAsync(x => x.Id == boardId);
            if (board == null) throw new ArgumentException("No board found for the given id");

            bool alreadyInBoard = board.Participants.Any(x => x.Profile.Id == profileId);
            if (alreadyInBoard) return;


            var profile = await _profileService.Queryable().FirstOrDefaultAsync(x => x.Id == profileId);
            if (profile == null) throw new ArgumentException("No profile found for the given id");

            var newParticipant = new Participant
            {
                Profile = profile,
                LastReadTimeStamp = DateTime.Now,
                TrackingState = TrackingState.Added
            };

            board.Participants.Add(newParticipant);
            board.TrackingState = TrackingState.Modified;

            _msgBoardService.ApplyChanges(board);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> CanAccessBoard(int profileId, int boardId)
        {
            return await _msgBoardService.Queryable()
                .AnyAsync(x => x.Id == boardId && x.Participants.Any(p => p.Profile.Id == profileId));
        }

        private ICollection<Participant> CreateBoardParticipants(ICollection<TeamMember> teamMembers)
        {
            var result = new List<Participant>();
            foreach (var member in teamMembers)
            {
                result.Add(new Participant
                {
                    Profile = member.MemberProfile,
                    LastReadTimeStamp = DateTime.Now.AddSeconds(-1),
                    TrackingState = TrackingState.Added
                });
            }

            return result;
        }
    }
}