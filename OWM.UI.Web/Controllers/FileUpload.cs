using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OWM.Application.Services.Interfaces;
using OWM.Domain.Entities;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    public class FileUpload : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        public FileUpload(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }
        [HttpPost("/user/profileimage")]
        public JsonResult Upload()
        {
            var file = Request.Form.Files[0];
            if (file.ContentType != "image/jpeg")
            {
                return Json(new { status = "error", message = "File Type Error" });
            }
            if (file.Length > 300000)
            {
                return Json(new { status = "error", message = "File Size Error" });
            }

            // Uncomment Below Code after user authentication was ok

            //var uploads = Path.Combine(hostingEnvironment.WebRootPath, "user_images");
            //var filePath = Path.Combine(uploads, Guid.NewGuid().ToString().Replace("-","") + ".jpg");
            //file.CopyTo(new FileStream(filePath, FileMode.Create));
            return Json(new { status = "success", message = "Succesfully Uploaded" });
        }
    }
}
