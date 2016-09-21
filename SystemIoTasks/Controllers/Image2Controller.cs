using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIoTasks.Controllers
{
    public class Image2Controller : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int maxHeight, int maxWidth)
        {
            if (file != null && file.ContentLength > 0) {

                Image image = Image.FromStream(file.InputStream);

                Image resizedImage = ResizeImage(image, maxWidth, maxHeight);

                //Laver så vi kan gemme data ned i RAM
                MemoryStream ms = new MemoryStream();

                resizedImage.Save(ms, ImageFormat.Png);


                //return File(ms.ToArray(), "image/png"); //retunere billede
                return View(ms); //retunere memorystream som model i Viewet
                

            }
            //Uploadéd file not found.
            return RedirectToAction("Index");
        }

        public static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / image.Width; //Tager maxWidth og dividere med billedes width
            double ratioY = (double)maxHeight / image.Height; //Tager maxHeight og dividere med billedes Højde
            double ratio = Math.Min(ratioX, ratioY); //Finder den laveste værdi af de 2 værdier

            int newWidth = (int)(image.Width * ratio); //difinere ny variable til Width baseret på billedet orginale width og ganger med ratio
            int newHeight = (int)(image.Height * ratio);//difinere ny variable til height baseret på billedet orginale height og ganger med ratio


            //Opretter nyt Bitmap billede med de nye dimensioner
            Bitmap newImage = new Bitmap(newWidth, newHeight);

            //Opretter et Graphics instance fra vores nye billede
            Graphics graphics = Graphics.FromImage(newImage);
            //Draw billedet på graphic objektet i pos 0,0 og størrelse efter det nye billede.
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
    }
}