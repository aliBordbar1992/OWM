namespace OWM.Application.Services.Dtos
{
    public class TeamMilesInformationDto
    {
        public string TeamName { get; set; }

        public string TeamCompletedMilesPercentage { get; set; }
        public float TeamCompletedMiles { get; set; }

        public float TeamTotalMilesPledged { get; set; }

        public string MyCompletedMilesPercentage { get; set; }

        public float MyCompletedMiles { get; set; }

        public float MyPledgedMiles { get; set; }

        public float MilesUntilCanComplete { get; set; }
        public string MilesUntilCanCompletePercentage { get; set; }

        public bool CanPledgeMiles { get; set; }
        public bool CanCompleteMiles { get; set; }
    }
}