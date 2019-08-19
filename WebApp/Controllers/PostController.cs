using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp
{
    public class PostController : Controller
    {
        private readonly MySqlContext context;
        private readonly ILogger<PostController> logger;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;



        public PostController(MySqlContext context, ILogger<PostController> logger, IHostingEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.logger = logger;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
        }
        public IActionResult UploadPicture()
        {
            return View(new Image());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UploadPicture(Image image)
        {
            ViewBag.Success = "";
            if (ModelState.IsValid)
            {
                FileStream stream=null;
                var user = userManager.GetUserAsync(HttpContext.User);
                string id = user.Result.Id;
                var filename = GetUniqueFileName(image.ImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/", filename);
                using (stream = new FileStream(path, FileMode.Create))
                {
                    image.ImageFile.CopyTo(stream);
                }
                image.ImagePath = filename;
                image.InsertAsync(id);
                ViewBag.Success = "Upload Successful";
                ModelState.Clear();
                return View();

            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                logger.LogError(errors.ToString());
            }
            return View(image);

        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        public IActionResult CategorizePicture()
        {
            Image i = new Image();
            var images = i.SelectAsync();
            ImageViewModel imageModel = new ImageViewModel()
            {
                Images = images,
                Image = new Image()
            };
            return View(imageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CategorizePicture(ImageViewModel imageModel)
        {
            var image = imageModel.Image;
            var id = image.ImageId;
            var category = image.Category;
            var type = image.Type;
            image.UpdateAsync(id,category,type);

            Image i = new Image();
            var images = i.SelectAsync();

            imageModel.Images = images;
            return View(imageModel);

        }

        public IActionResult DeletePicture(int imageId){
            int zid = imageId;
            Image i = new Image();
            i.DeleteAsync(imageId);
            return RedirectToAction("CategorizePicture", "Post");

        }
    }
}
