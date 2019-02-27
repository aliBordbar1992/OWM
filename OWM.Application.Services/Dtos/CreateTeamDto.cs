using System;
using System.Collections.Generic;
using System.Linq;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;

namespace OWM.Application.Services.Dtos
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AgeRange Range { get; set; }
        public bool OccupationFilter { get; set; }
        public string Occupations { get; set; }
        public List<Occupation> OccupationsList => string.IsNullOrEmpty(Occupations)
            ? new List<Occupation>()
            : Occupations.Split(',').Select(x => new Occupation { Name = x }).ToList();
    }
}