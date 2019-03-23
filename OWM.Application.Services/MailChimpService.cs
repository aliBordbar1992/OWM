using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Microsoft.Extensions.Configuration;
using OWM.Application.Services.Dtos;
using OWM.Application.Services.Interfaces;

namespace OWM.Application.Services
{
    public class MailChimpService : MailChimpInit, IMailChimpService
    {
        public async Task AddMemberToList(MailChimpMemberDto member)
        {
            var t = await
                this.MailChimpManager.Members.AddOrUpdateAsync(
                    ListId,
                    new Member
                    {
                        EmailAddress = member.Email,
                        Status = Status.Subscribed,
                        MergeFields = new System.Collections.Generic.Dictionary<string, object>{
                            { "FNAME", member.FirstName },
                            { "LNAME", member.LastName },
                            { "BIRTHDAY", member.Birthday },
                            { "PHONE", member.Phone },
                            { "OCCUPTN", member.Occupation },
                            { "ETHNIC", member.Ethnicity },
                            { "MINTERESTS", member.Interests },
                            { "COUNTRY", member.CountryName },
                            { "GENDER", member.Gender },
                            { "HOWDIDKNOW", member.HowDidYouHearUs },
                            { "CITY", member.CityName }
                        }
                    }).ConfigureAwait(true);

            var updateMergeField =
                await

                    this.MailChimpManager.Members.AddOrUpdateAsync(
                        ListId, t).ConfigureAwait(false);

        }
    }

    public abstract class MailChimpInit
    {
        protected IMailChimpManager MailChimpManager;
        protected string ListId => "d3b0291dff";
        protected MailChimpInit()
        {
            this.MailChimpManager = new MailChimpManager("3116ed530a8f14dc21d4536268564f76-us20");
        }
    }
}