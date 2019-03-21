namespace OWM.Application.Services.Dtos
{
    public class TeamBoardsDto
    {
        public int BoardId { get; set; }
        public string TeamName { get; set; }
        public bool HasUnreadMessages { get; set; }
        public int UnreadMessages { get; set; }
        public bool Selected { get; set; }
    }
}