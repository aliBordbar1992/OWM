namespace OWM.Application.Services.Dtos
{
    public class CanJoinTeamDto
    {
        public bool IsLoggedIn { get; set; }
        public bool OccupationFilter { get; set; }
        public bool OccupationsMatch { get; set; }
        public bool AgeRangeMatch { get; set; }
        public bool TeamIsClosed { get; set; }
        public bool IsAlreadyMember { get; set; }
        public bool FinalResult { get; set; }
    }
}