using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Exceptions;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions;

namespace OWM.Application.Services
{
    public class TeamMessageBoardService
    {
        private readonly ITeamService _teamService;
        private readonly IMessageBoardService _msgBoardService;
        private readonly IUnitOfWork _unitOfWork;

        public TeamMessageBoardService(ITeamService teamService
            , IMessageBoardService messageBoardService
            , IUnitOfWork unitOfWork)
        {
            _teamService = teamService;
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

        private ICollection<Participant> CreateBoardParticipants(ICollection<TeamMember> teamMembers)
        {
            var result = new List<Participant>();
            foreach (var member in teamMembers)
            {
                result.Add(new Participant
                {
                    Profile = member.MemberProfile,
                    LastReadTimeStamp = null,
                    TrackingState = TrackingState.Added
                });
            }

            return result;
        }
    }

    public class ParticipantInforationDto
    {
        public string ProfilePicture { get; set; }
        public string FullName { get; set; }
        public int ProfileId { get; set; }
    }
}