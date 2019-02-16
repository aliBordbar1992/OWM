using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWM.UI.Web.Controllers
{
    public class StatisticData
    {
        public static StatData GetStatisticData()
        {
            var statData = new StatData {Participants = 25, Countries = 3, MilesPledged = 255};
            return statData;
        }
        public class StatData 
        {
            public int Participants { get; set; }
            public int Countries { get; set; }
            public int MilesPledged { get; set; }
        }
    }
}
