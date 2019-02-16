using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWM.UI.Web.Controllers
{
    public class TeamManagment
    {
        public static List<MyTeams> GeTeams()
        {
            if (!CheckUserLogin.IsAuthenticated()) return null;
            var teamList = new List<MyTeams>();
            for (int i = 0; i < 3; i++)
            {
                teamList.Add(new MyTeams()
                {
                    TeamName = "Omid'sTeam" + i,
                    MilesPledgedMe = 2 + i,
                    MilesPledgedTeam = 5 + i,
                    MilesCompletedMe = 6 + i,
                    MilesCompletedTeam = 10 + i,
                    TeamId = Guid.NewGuid().ToString().Replace("-","")
                });
            }
            return teamList;
        }
        public class MyTeams
        {
            public string TeamName { get; set; }
            public int MilesPledgedMe { get; set; }
            public int MilesPledgedTeam { get; set; }
            public int MilesCompletedMe { get; set; }
            public int MilesCompletedTeam { get; set; }
            public string TeamId { get; set; }
        }
    }
}
