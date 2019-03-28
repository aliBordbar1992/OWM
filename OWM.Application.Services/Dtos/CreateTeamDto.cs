using OWM.Domain.Entities.Enums;

namespace OWM.Application.Services.Dtos
{
    public class CreateTeamDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AgeRange Range { get; set; }
        public bool OccupationFilter { get; set; }
        public int[] OccupationIds { get; set; }
        public int ProfileId { get; set; }
    }
}