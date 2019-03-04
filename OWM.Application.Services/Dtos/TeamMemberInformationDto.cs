namespace OWM.Application.Services.Dtos
{
    public class TeamMemberInformationDto
    {
        public int TeamId { get; set; }
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
        public float MyCompletedMiles { get; set; }
        public float MyPledgedMiles { get; set; }
        public bool IsCreator { get; set; }
        public bool IsKickedOut { get; set; }
    }
}