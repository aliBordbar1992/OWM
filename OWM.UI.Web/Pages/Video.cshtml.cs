using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.PlatformAbstractions;

namespace OWM.UI.Web.Pages
{
    public class Video : PageModel
    {
        private readonly IHostingEnvironment _hostingEnv;
        public Video(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public void OnGet()
        {
            videos = new List<VideoFiles>();

            string path = Path.Combine(_hostingEnv.WebRootPath, "videos\\gallery\\");
            
            foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                string title = Path.GetFileNameWithoutExtension(file);
                videos.Add(new VideoFiles
                {
                    Title = title,
                    Address = $"/videos/gallery/{Path.GetFileName(file)}"
                });
            }
        }

        public List<VideoFiles> videos { get; set; }

        public class VideoFiles
        {
            public string Title { get; set; }
            public string Address { get; set; }
        }
    }
}