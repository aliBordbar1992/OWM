using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    public class test:Controller
    {
        private readonly IUserRegistrationService _userRegistrationService;
        public List<string> EthnicityOptions;
        public test(IUserRegistrationService userRegistrationService)
        {
            _userRegistrationService = userRegistrationService;
            EthnicityOptions = new List<string>();
        }
        [HttpPost("/eth/all")]
        public JsonResult Get(string str)
        {
            var b = _userRegistrationService.GetEthnicities().ToList();
            var c = new List<Ethnicity>();
            foreach (var i in b.Result)
            {
                c.Add(new Ethnicity()
                {
                    Id = i.Id,
                    Name = i.Name
                });
            }
            return new JsonResult(c); 
        }
    }
}
