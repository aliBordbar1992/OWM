using System.Collections.Generic;

namespace OWM.Application.Services.Dtos
{
    public class ProfileInformationDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Occupation { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public float MilesPledged { get; set; }
        public float MilesCompleted { get; set; }
        public List<string> TeamsCreated { get; set; }
    }
}