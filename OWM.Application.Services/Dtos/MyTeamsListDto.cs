namespace OWM.Application.Services.Dtos
{
    public class MyTeamsListDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public float MyPledgedMiles { get; set; }
        public float MyCompletedMiles { get; set; }
        public float TotalMilesCompleted { get; set; }
        public float TotalMilesPledged { get; set; }
        public bool IsCreator { get; set; }
    }
}