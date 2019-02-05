using System;

namespace OWM.Application.Services.Dtos
{
    public class UserRegistrationDto
    {
        public int OccupationId { get; set; }
        public int CityId { get; set; }
        public int EthnicityId { get; set; }
        public int Gender { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}