using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace OWM.UI.Web.Controllers
{
    [Route("api/[Controller]")]
    public class FileUpload : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public FileUpload(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
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
