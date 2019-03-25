using System;
using System.Collections.Generic;
using OWM.Application.Services.Utils;

namespace OWM.Application.Services.Dtos
{
    public class TeamInformationDto
    {
        public int TeamId { get; set; }
        public int BoardId { get; set; }
        public int TeamMembersCount { get; set; }

        public string TeamName { get; set; }
        public string str_DateCreated => DateCreated.ToString(Constants.DateFormat_LongMonth);
        public string str_TotalMilesPledged { get; set; }
        public string str_TotalMilesCompleted { get; set; }
        public string TeamCompletedMilesPercentage { get; set; }
        public string Description { get; set; }

        public float TotalMilesPledged { get; set; }
        public float TotalMilesCompleted { get; set; }

        public bool IsClosed { get; set; }

        public DateTime DateCreated { get; set; }

        public List<string> Occupations { get; set; }
        public List<TeamMemberInformationDto> TeamMembers { get; set; }
    }
}