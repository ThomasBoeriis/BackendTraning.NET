using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace SystemIoTasks.Controllers
{
    public class BarchartController : Controller
    {
        // GET: Barchart
        public ActionResult Index(string[] xAxis, int[] yAxis)
        {

            //Options for barsene
            int spacing = 40;
            int barWidth = 20;
            int barScale = 10;
            int maxHeight = 0;
            int maxWidth = 0;

            Graphics gWidth = Graphics.FromImage(new Bitmap(1, 1));

            //Find height ud fra den største værdig i yAxis arrayét og plus 40 for ekstra space
            maxHeight = yAxis.Max() * barScale + 40;

            //Find den value i xAxis der har det længeste navn når det bliver skrevet ud
            foreach(string value in xAxis)
            {
                if(gWidth.MeasureString(value.ToString(), new Font("Courier New", 10, FontStyle.Italic)).Width > maxWidth)
                {
                    maxWidth = (int)gWidth.MeasureString(value.ToString(), new Font("Courier New", 10, FontStyle.Italic)).Width;
                }
            }

            //Find den value i yAxis der har det længeste navn når det bliver skrevet ud
            foreach (int value in yAxis)
            {
                if (gWidth.MeasureString(value.ToString(), new Font("Courier New", 10, FontStyle.Italic)).Width > maxWidth)
                {
                    maxWidth = (int)gWidth.MeasureString(value.ToString(), new Font("Courier New", 10, FontStyle.Italic)).Width;
                }
            }


            barWidth = maxWidth;


            //opret nyt bitmap og graphics object baseret på størrelse på array elementerne
            Bitmap bitmap = new Bitmap((xAxis.Length * barWidth) + (xAxis.Length * spacing) + (spacing / 2), maxHeight);
            Graphics gfx = Graphics.FromImage(bitmap);

            //Sætter baggrundsfarven
            gfx.Clear(Color.DarkGray);

            int i = 0;
            foreach(int value in yAxis)
            {
                //gfx.DrawRectangle(Pens.Black, (i * spacing) + (i* barWidth) + 15, maxHeight - (value * barScale),
            }





            MemoryStream ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Png);

            return base.File(ms.ToArray(), "image/png");

        }
    }
}