using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemIoTasks.Models;

namespace SystemIoTasks.Controllers
{
    public class GalleryController : Controller
    {
        // GET: Gallery
        public ActionResult Index(string folder)
        {

            string rootPath = Server.MapPath("~/GalleryData");

            GalleryViewModel model = new GalleryViewModel();

            //1. List alle mapper ud
            model.RootFolders = new List<DirectoryInfo>();
            foreach (string dirPath in Directory.GetDirectories(rootPath).ToList())
            {
                model.RootFolders.Add(new DirectoryInfo(dirPath));
            }

            //Hvis folder ikke er null  sæt currentFolder til folder og find alle filer i denne mappe
            if (folder != null)
            {
                model.CurrentFolder = folder;
                if (Directory.Exists(rootPath + "/" + folder))
                {
                    model.FilesInCurrentFolder = new List<FileInfo>();

                    foreach (string file in Directory.GetFiles(rootPath + "/" + folder))
                    {
                        model.FilesInCurrentFolder.Add(new FileInfo(file));
                    }
                }

            }


            return View(model);
        }



        [HttpPost]
        public ActionResult UploadFiles(List<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                string CurrentDayFolder = DateTime.Now.ToString("dd-MM-yyyy");

                foreach (var file in files)
                {

                    if (file.ContentLength > 0)
                    {
                        if (ApprovedTypes.Contains(Path.GetExtension(file.FileName)))
                        {
                            string path = Server.MapPath("~/GalleryData");

                            string fileName = Path.GetFileName(file.FileName);
                            string fileExtension = Path.GetExtension(file.FileName);
                            //mappen skal eksistere ellers kommer der fejl.
                            string SaveLocation = Path.Combine(path, CurrentDayFolder, Guid.NewGuid() + fileExtension);

                            //Opret mappe hvis den ikke eksistere
                            if (!Directory.Exists(Path.Combine(path, CurrentDayFolder)))
                            {
                                Directory.CreateDirectory(Path.Combine(path, CurrentDayFolder));
                            }
                            try
                            {
                                file.SaveAs(SaveLocation);
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("", "Error: " + ex.Message);
                            }
                        }
                        else
                        {
                            TempData.Add("Message", "File Type not supported.");
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("", "Please select a file to upload.");
                    }
                }
                TempData.Add("Message", "The files has been uploaded.");

            }
            else
            {
                ModelState.AddModelError("", "Please select a file to upload.");
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteFile(string file)
        {
            //Find stien til filen
            string rootPath = Server.MapPath("~/GalleryData");
            string path = Path.Combine(rootPath, file);

            string currentFolder = file.Split('\\').First();

            //Få fat i filen via stien
            FileInfo fileInfo = new FileInfo(path);

            //Prøv og slet hvis det fejler send fejlbesked til TempData
            try
            {
                fileInfo.Delete();
                TempData.Add("Message", "The file has been deleted.");
            }
            catch (Exception e)
            {
                TempData.Add("Message", e.Message);
            }

            //Retunere til vores Action ListFilesAndInfo
            return RedirectToAction("Index", new { folder = currentFolder });
        }


        public ActionResult GetThumbnail(string file, int width, int height)
        {
            string rootPath = Server.MapPath("~/GalleryData");
            string path = Path.Combine(rootPath, file);

            Image thisImage = Image.FromFile(path);

            

            Image resizedImage = thisImage.GetThumbnailImage(width, height, null, IntPtr.Zero);
            

            MemoryStream ms = new MemoryStream();

            resizedImage.Save(ms, ImageFormat.Png);

            return base.File(ms.ToArray(), "image/png");
            
        }

        public ActionResult ImageScale(string file, int width, int height)
        {
            string rootPath = Server.MapPath("~/GalleryData");
            string path = Path.Combine(rootPath, file);

            //Resize image fra en fil.
            //MÅske man skulle tjekke på om filen eksistere først før man gør dette.
            Image newImage = ModifyImage(Image.FromFile(path), width, height);

            //Opretter en Stream så vi kan stream vores billed ud
            MemoryStream ms = new MemoryStream();
            //Gem billedet i vores stream i formatet png
            newImage.Save(ms, ImageFormat.Png);

            //returnere det vi får som fil via vores stream, som image/png format.
            return File(ms.ToArray(), "image/png");

        }


        public static Image ModifyImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            var graphics = Graphics.FromImage(newImage);
            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            // Create font and brush.
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Far,
                LineAlignment = StringAlignment.Far
            };

            graphics.DrawString("@Copyright ThomasBoeriis", drawFont, drawBrush, newWidth, newHeight, format);

            return newImage;
        }


        public static List<string> ApprovedTypes = new List<string>()
        {
            ".jpg",".gif",".png"
        };
    }
}