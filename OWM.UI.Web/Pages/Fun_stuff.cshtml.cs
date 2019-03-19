using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OWM.UI.Web.Pages
{
    public class Fun_stuff : PageModel
    {
        private readonly IHostingEnvironment _hostingEnv;
        public Fun_stuff(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public void OnGet()
        {
            pictures = new List<PictureFiles>();

            string path = Path.Combine(_hostingEnv.WebRootPath, "funstuff");

            foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                string title = Path.GetFileNameWithoutExtension(file);
                pictures.Add(new PictureFiles
                {
                    Title = title,
                    Address = $"/funstuff/{Path.GetFileName(file)}"
                });
            }
        }

        public List<PictureFiles> pictures { get; set; }

        public class PictureFiles
        {
            public string Title { get; set; }
            public string Address { get; set; }
        }
    }
}