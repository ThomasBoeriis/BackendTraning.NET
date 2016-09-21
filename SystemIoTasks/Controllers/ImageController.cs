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
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int maxHeight, int maxWidth)
        {
            if(file != null && file.ContentLength > 0)
            {
                Image image = Image.FromStream(file.InputStream);

                Image resizedImage = ResizeImage(image, maxWidth, maxHeight);

                MemoryStream ms = new MemoryStream();

                resizedImage.Save(ms, ImageFormat.Png);

                return View(ms);
            }

            return View();
        }


        // GET: Image
        public ActionResult Photostrip()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Photostrip(List<HttpPostedFileBase> files, int maxWidth)
        {
            //Options
            int spacing = 50;
            int boxSize = spacing/3; //bliver 1/3 af spacing
            Color backgroundColor = Color.Black;

            int totalHeight = spacing;
            List<ImageInformation> resizedImages = new List<ImageInformation>();
            foreach (var file in files)
            {
                if(file != null && file.ContentLength > 0)
                {

                    Image image = Image.FromStream(file.InputStream);

                    Image resizedImage = ResizeImage(image, maxWidth - (2*spacing));
                    resizedImages.Add(new ImageInformation()
                    {
                        Image = resizedImage,
                        FileName = file.FileName
                    });

                    totalHeight += resizedImage.Height + spacing;

                }
            }

            Bitmap canvas = new Bitmap(maxWidth, totalHeight);
            Graphics graphics = Graphics.FromImage(canvas);

            //Sætter baggrundsfarven
            graphics.Clear(backgroundColor);

            int nextImageYPos = spacing;
            foreach(ImageInformation img in resizedImages)
            {
                graphics.DrawImage(img.Image,spacing,nextImageYPos);
                var text = graphics.MeasureString(img.FileName, new Font("Arial", 10));
                
                SolidBrush textBrush = new SolidBrush(Color.White);

                graphics.DrawString(
                    img.FileName,
                    new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Point), 
                    textBrush,
                    22 ,
                    img.Image.Height - text.Height);

                nextImageYPos += img.Image.Height+spacing;

            }

            int boxAmount = totalHeight / (boxSize * 2);
            for (int i = 0; i < boxAmount; i++)
            {
                //opret kasse i graphics element
                SolidBrush brush = new SolidBrush(Color.White);

                Rectangle left = new Rectangle(
                    new Point(boxSize, (i * boxSize) + boxSize + (i*boxSize)), 
                    new Size(boxSize, boxSize)
                );
                Rectangle right = new Rectangle(
                    new Point(maxWidth - (boxSize*2), (i * boxSize) + boxSize + (i * boxSize)),
                    new Size(boxSize, boxSize)
                );

                graphics.FillRectangle(brush, left);
                graphics.FillRectangle(brush, right);
            }

            //Laver så vi kan gemme data ned i RAM
            MemoryStream ms = new MemoryStream();

            canvas.Save(ms, ImageFormat.Png);

            //retunere billede
            return File(ms.ToArray(), "image/png"); 
        }


        public static Image ResizeImage(Image image, int maxWidth)
        {
            double ratio = (double)maxWidth / image.Width;


            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            Bitmap newImage = new Bitmap(newWidth, newHeight);
            Graphics graphics = Graphics.FromImage(newImage);

            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;

        }

        public static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;

            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            Bitmap newImage = new Bitmap(newWidth, newHeight);
            Graphics graphics = Graphics.FromImage(newImage);

            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;

        }
    }


    
}