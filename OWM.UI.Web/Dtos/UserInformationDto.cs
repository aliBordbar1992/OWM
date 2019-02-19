using OWM.Application.Services.Dtos;

namespace OWM.UI.Web.Dtos
{
    public class UserInformationDto : UserRegistrationDto
    {
        public string UserImage { get; set; }
        public string Occupation { get; set; }
        public string Ethnicity { get; set; }
        public string TeamJoined { get; set; }
        public string MilesPledged { get; set; }
        public string MilesCompleted { get; set; }
    }
}
