using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    public class PageController : Controller
    {
        public PageController()
        {
        }

        

        public IActionResult ViewPage(string category){
            string c = category;
            Image i = new Image();
            List<Image> images = i.SelectByType(category);
            ImageViewModel imageModel = new ImageViewModel
            {
                Images = images,
                Image = i
            };

            return View(imageModel);
        }
        public ActionResult Download(string imagePath){
            var absolutePath = "/Users/Merry/Projects/WebApp/WebApp/wwwroot/uploads/"+imagePath;
            return PhysicalFile(absolutePath, "image/*", imagePath);


        }

    }
}
