using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(Roles = "User")]
    public class FileUpload : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        public FileUpload(IHostingEnvironment environment)
        {
            _hostingEnv = environment;
        }
        [HttpPost("/user/profileimage")]
        public JsonResult Upload()
        {
            var file = Request.Form.Files[0];
            if (file.ContentType != "image/jpeg") return Json(new {status = "error", message = "File type error"});
            if (file.Length > 300000) return Json(new {status = "error", message = "File size must be less than 300kb"});


            try
            {
                var uploads = Path.Combine(_hostingEnv.WebRootPath, "user_images");
                string fileName = $"{Guid.NewGuid().ToString().Replace("-", "")}.jpg";
                var filePath = Path.Combine(uploads, fileName);
                file.CopyTo(new FileStream(filePath, FileMode.Create));

                return Json(new {status = "success", message = "Succesfully uploaded", picAddress = $"/user_images/{fileName}"});
            }
            catch (Exception e)
            {
                return Json(new { status = "error", message = $"{e.Message}. Try again." });
            }
        }
    }
}
