using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OWM.UI.Web.Areas.Identity.Data;
using OWM.UI.Web.Models;

[assembly: HostingStartup(typeof(OWM.UI.Web.Areas.Identity.IdentityHostingStartup))]
namespace OWM.UI.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<OWMUIWebContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("OWMUIWebContextConnection")));

                services.AddDefaultIdentity<OWMUIWebUser>()
                    .AddEntityFrameworkStores<OWMUIWebContext>();
            });
        }
    }
}