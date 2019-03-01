namespace OWM.Application.Services.Dtos
{
    public class MyTeamsListDto
    {
        public string TeamName { get; set; }
        public float MyPledgedMiles { get; set; }
        public float TotalMilesPledged { get; set; }
        public float MyCompletedMiles { get; set; }
        public float TotalMilesCompleted { get; set; }
        public int TeamId { get; set; }
    }
}