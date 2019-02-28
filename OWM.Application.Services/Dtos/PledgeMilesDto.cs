namespace OWM.Application.Services.Dtos
{
    public class PledgeMilesDto
    {
        public PledgeMilesDto(int teamId, int profileId, float miles)
        {
            TeamId = teamId;
            ProfileId = profileId;
            Miles = miles;
        }

        public int TeamId { get; set; }
        public int ProfileId { get; set; }
        public float Miles { get; set; }
    }
}