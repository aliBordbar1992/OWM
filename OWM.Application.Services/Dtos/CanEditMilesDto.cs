namespace OWM.Application.Services.Dtos
{
    public class CanEditMilesDto
    {
        public bool IsUnder26Miles { get; set; }
        public bool IsUnderCompletedMiles { get; set; }
        public bool NoOtherMemberHaveCompletedMiles { get; set; }
        public bool CanEditMiles { get; set; }
    }
}