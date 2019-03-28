using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    public class FilterCountriesByCities:Controller
    {
        [HttpGet("/api/FilterCountriesByCities")]
        public IActionResult Get(string city)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://gd.geobytes.com/AutoCompleteCity?callback=?&sort=size&q=" + city);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return new JsonResult(responseString);
        }

        [HttpGet("/api/GetCountry")]
        public IActionResult GetCountry(string country)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://gd.geobytes.com/GetCityDetails?callback=?&fqcn=" + country);
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return new JsonResult(responseString);
        }
    }
}