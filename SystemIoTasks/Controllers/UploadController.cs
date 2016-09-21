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
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexOld(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0))
            {
                string fileName = Path.GetFileName(file.FileName);

                //mappen skal eksistere ellers kommer der fejl.
                string SaveLocation = Server.MapPath("Data") + "\\" + fileName;
                try
                {
                    file.SaveAs(SaveLocation);
                    TempData.Add("Message", "The file has been uploaded.");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select a file to upload.");
            }

            return View();
        }

        //[HttpPost]
        //public ActionResult Index(HttpPostedFileBase file)
        //{
        //    if ((file != null) && (file.ContentLength > 0))
        //    {

        //        string path = Server.MapPath("Data");

        //        string fileName = Path.GetFileName(file.FileName);
        //        //mappen skal eksistere ellers kommer der fejl.
        //        string SaveLocation = path + "\\" + fileName;

        //        //Opret mappe hvis den ikke eksistere
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        try
        //        {
        //            file.SaveAs(SaveLocation);
        //            TempData.Add("Message", "The file has been uploaded.");

        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", "Error: " + ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Please select a file to upload.");
        //    }

        //    return View();
        //}


        [HttpPost]
        public ActionResult Index(List<HttpPostedFileBase> files, string name)
        {

            foreach (var file in files)
            {

                if ((file != null) && (file.ContentLength > 0))
                {

                    string path = Server.MapPath("~/Data/Images");
                    //mappen skal eksistere ellers kommer der fejl.
                    string SaveLocation = Path.Combine(path, file.FileName);


                    //Opret mappe hvis den ikke eksistere
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    Image image = Image.FromStream(file.InputStream);

                    Image copyright = Image.FromFile(Server.MapPath("~/Data/copyright.jpg"));

                    Graphics graphics = Graphics.FromImage(image);

                    Font font = new Font("Open Sans", 35, FontStyle.Bold);
                    SolidBrush brush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
                    StringFormat stringformat = new StringFormat()
                    {
                        Alignment = StringAlignment.Far,
                        LineAlignment = StringAlignment.Far
                    };

                    graphics.DrawString(name, font, brush, image.Width, image.Height, stringformat);

                    //SolidBrush brush2 = new SolidBrush(Color.Red);
                    //Rectangle rect = new Rectangle(image.Width-(200), image.Height-(200), 200, 200);

                    //graphics.FillRectangle(brush2, rect);

                    graphics.DrawImage(
                        copyright, 
                        ((image.Width / 2) - copyright.Width /2), 
                        ((image.Height / 2) - copyright.Height/2)
                    );


                    string format = Path.GetExtension(file.FileName);

                    switch (format)
                    {
                        case ".jpg":
                            image.Save(SaveLocation, ImageFormat.Jpeg);
                            break;
                        case ".png":
                            image.Save(SaveLocation, ImageFormat.Png);
                            break;
                        case ".gif":
                            image.Save(SaveLocation, ImageFormat.Gif);
                            break;
                    }



                }

            }

            return View();
        }
    }
}