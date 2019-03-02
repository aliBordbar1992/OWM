using System;
using System.Collections.Generic;

namespace OWM.Application.Services.Dtos
{
    public class TeamInformationDto
    {
        public string TeamName { get; set; }
        public DateTime DateCreated { get; set; }
        public List<string> Occupations { get; set; }
        public bool IsClosed { get; set; }
        public float TotalMilesPledged { get; set; }
        public float TotalMilesCompleted { get; set; }
        public List<TeamMemberInformationDto> TeamMembers { get; set; }
        public string Description { get; set; }
    }
}