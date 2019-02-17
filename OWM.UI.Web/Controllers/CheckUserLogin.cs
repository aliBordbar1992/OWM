using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.Dtos;

namespace OWM.UI.Web.Controllers
{
    public class CheckUserLogin
    {
        public static UserData GetUserData()
        {
            if (!IsAuthenticated()) return null;
            var userData = new UserData()
            {
                Name = "Omid",
                Surname = "Mokhtari",
                CityId = 5,
                CityName = "Tehran",
                Birthday = "01/15/1993",
                OccupationId = 2,
                CountryName = "Iran",
                Email = "Omid@gmail.com",
                EthnicityId = 5,
                Gender = 2,
                Phone = "09190152706",
                UserImage = "http://square-vn.com/app/dscms/assets/images/person-4.jpg?v=1495618120",
                Occupation = "Actor",
                Ethnicity = "Asian",
                MilesCompleted = "12",
                MilesPledged = "3.5",
                TeamJoined = "2"
            };
            return userData;
        }

        public static bool IsAuthenticated()
        {
            return false;
        }
        
        public class UserData:UserRegistrationDto
        {
            public string UserImage { get; set; }
            public string Occupation { get; set; }
            public string Ethnicity { get; set; }
            public string TeamJoined { get; set; }
            public string MilesPledged { get; set; }
            public string MilesCompleted { get; set; }
        }
    }
}
