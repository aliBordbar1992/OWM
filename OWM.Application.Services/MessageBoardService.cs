using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OWM.Application.Services.Exceptions;
using OWM.Domain.Entities;
using OWM.Domain.Services.Interfaces;
using TrackableEntities.Common.Core;

namespace OWM.Application.Services
{
    public class MessageBoardService
    {
        private readonly ITeamService _teamService;

        public async Task<int> GetOrCreateTeamBoard(int teamId)
        {
            var team = await _teamService.Queryable()
                .FirstOrDefaultAsync(x => x.Id == teamId);
            if (team == null)
                throw new TeamNotFoundException<int>($"No team found for id {teamId}", null, teamId);

            //var board = null;//team.Board;

            //if (board == null)
            //{
            //    board = new MessageBoard
            //    {
            //        ForTeam = team,
            //        Participants = CreateBoardParticipants(team.Members),
            //        TrackingState = TrackingState.Added
            //    };


            //}

            return -1;
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
}