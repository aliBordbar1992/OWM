using System;
using System.Collections.Generic;
using System.Linq;
using OWM.Domain.Entities;

namespace OWM.Application.Services.Dtos
{
    public class UserInformationDto
    {
        public int ProfileId { get; set; }
        public int OccupationId { get; set; }
        public int OccupationOrder { get; set; }
        public int CityId { get; set; }
        public int EthnicityId { get; set; }
        public int Gender { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string Interest { get; set; }
        public string Birthday { get; set; }
        public string ProfilePicture { get; set; }
        public string Occupation { get; set; }
        public string Ethnicity { get; set; }
        public string TeamJoined { get; set; }
        public string MilesPledged { get; set; }
        public string MilesCompleted { get; set; }

        public int InvitationCount { get; set; }
        public bool HasInvitations { get; set; }

        public int UnreadMessagesCount { get; set; }
        public bool HasMessages { get; set; }

        public bool IsLoggedIn { get; set; }

        public List<Interest> Interests => string.IsNullOrEmpty(Interest)
            ? new List<Interest>()
            : Interest.Split(',').Select(x => new Interest {Name = x}).ToList();

        public DateTime? DateOfBirth => string.IsNullOrEmpty(Birthday)
            ? (DateTime?) null
            : DateTime.ParseExact(Birthday.Replace("-", "/"), Utils.Constants.DateFormat, null);

        public UserInformationDto()
        {
            IsLoggedIn = true;
        }

        public UserInformationDto(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
        }
    }
}
