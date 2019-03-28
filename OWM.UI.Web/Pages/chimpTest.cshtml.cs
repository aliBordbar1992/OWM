using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.UI.Web.Pages
{
    public class chimpTestModel : PageModel
    {
        private readonly IMailChimpService _chimpService;

        public chimpTestModel(IMailChimpService chimpService)
        {
            _chimpService = chimpService;
        }

        public void OnGet()
        {
            MailChimpMemberDto newMember = new MailChimpMemberDto
            {
                FirstName = "test",
                LastName = "api",
                Occupation = "I prefer not to say",
                Interests = "something",
                CityName = "london",
                CountryName = "United kingdom",
                HowDidYouHearUs = "friend invitation",
                Phone = "+989372346281",
                Email = "fijohigi@placemail.online",
                Gender = "Male",
                Ethnicity = "African",
                Birthday = new DateTime(1985,10,10).ToString("MM/dd"),
            };

            _chimpService.AddMemberToList(newMember);
        }
    }
}