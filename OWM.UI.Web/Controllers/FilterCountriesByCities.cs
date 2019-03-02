using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var start = responseString.IndexOf("(") + 1;
            var end = responseString.IndexOf(")", start);
            responseString = responseString.Substring(start, end - start);
            var js = JsonConvert.DeserializeObject<List<string>>(responseString);
            return new JsonResult(js);
        }

        [HttpGet("/api/GetCountry")]
        public IActionResult GetCountry()
        {
            return null;
        }
    }
}