using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            responseString = responseString.Replace("?", "");
            responseString = responseString.Replace("([", "");
            responseString = responseString.Replace("]);", "");
            responseString = responseString.Remove(responseString.LastIndexOf("\""));
            responseString = responseString.Remove(0, 1);
            var array = responseString.Split("\"");
            array = array.Where(x => x != ",").ToArray();
            return new JsonResult(array); 
        }

        [HttpGet("/api/GetCountry")]
        public IActionResult GetCountry()
        {
            return null;
        }
    }
}
