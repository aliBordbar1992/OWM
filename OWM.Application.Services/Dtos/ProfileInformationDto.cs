using System.Collections.Generic;

namespace OWM.Application.Services.Dtos
{
    public class ProfileInformationDto
    {
        public int OccupationOrder { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ProfilePicture { get; set; }
        public string Occupation { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string MilesPledged { get; set; }
        public string MilesCompleted { get; set; }
        public List<TeamInfoDto> TeamsCreated { get; set; }
        public List<TeamInfoDto> TeamsJoined { get; set; }
        public List<string> Interests { get; set; }
    }

    public class TeamInfoDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
    }
}