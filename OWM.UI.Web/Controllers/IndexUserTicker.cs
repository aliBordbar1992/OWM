using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWM.UI.Web.Controllers
{
    public class IndexUserTicker
    {
        public static string[] GetTickerData()
        {
            var teckerData = new[]
            {
                "Maggie from Harleysville, US just pledged 5.03 miles",
                "Suzanne from Harleysville, US just pledged 10 miles",
                "Kevin from Harleysville, US just pledged 13.01 miles",
                "Gabrielle from Harleysville, US just pledged 5.01 miles",
                "Madison from Harleysville, US just pledged 13.1 miles",
                "nic from Philadelphia, US just pledged 20.13 miles",
                "Tom-James from Lancaster, GB just pledged 10.69 miles",
                "Jonny from Manchester, GB just pledged 0.99 miles",
                "Alan from London, GB just pledged 1.24 miles",
                "Will from United Kingdom just pledged 3.11 miles"
            };
            return teckerData;
        }
        
    }
}
