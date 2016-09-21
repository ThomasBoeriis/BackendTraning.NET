using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using SystemIoTasks.Models;
using System.Drawing;

namespace SystemIoTasks.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files
        public ActionResult Index()
        {
            //List alle Filer ud fra en mappe
            List<string> files = Directory.GetFiles(Server.MapPath("~/Data/Images")).ToList();



            return View(files);
        }


        public ActionResult ListFilesAndInfo()
        {
            //List alle Filer ud fra en mappe
            List<string> files = Directory.GetFiles(Server.MapPath("~/Data/Images")).ToList();

            //Opret ny Liste til at indeholde fileInfo
            List<FileInfo> fileWithInfo = new List<FileInfo>();
            List<Image> Images = new List<Image>();
            foreach (string file in files)
            {
                fileWithInfo.Add(new FileInfo(file));

                Images.Add(Image.FromFile(file));
            }
            return View(fileWithInfo);
        }

        public ActionResult DeleteFile(string file)
        {
            //Find stien til filen
            string path = Server.MapPath("~/Data/Images/" + file);

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
            return RedirectToAction("ListFilesAndInfo");
        }


        public ActionResult ReadFile(string name)
        {
            //Opret ny ViewModel
            ReadFileViewModel model = new ReadFileViewModel(); 

            //Vis filer 
            List<string> files = Directory.GetFiles(Server.MapPath("~/Data/Text")).ToList();

            //Opret ny Liste til at indeholde fileInfo
            foreach (string file in files)
            {
                model.Files.Add(new FileInfo(file));
            }

            if (name != null)
            {
                //Read file and return it as a string
                string path = Server.MapPath("~/Data/Text/" + name);
                FileInfo fileInfo = new FileInfo(path);

                if (fileInfo.Exists)
                {
                    // Read the file and display it line by line.
                    StreamReader file = new StreamReader(path);
                    model.Message = file.ReadToEnd();
                    file.Close();
                    return View(model);
                }
                else
                {
                    TempData.Add("Message", "File not found");

                }

            }
            return View(model);

        }

        [HttpPost]
        public ActionResult UpdateFile(string name, string message)
        {
            if (name != null)
            {
                //Read file and return it as a string
                string path = Server.MapPath("~/Data/Text/" + name);
                FileInfo fileInfo = new FileInfo(path);

                
                if (fileInfo.Exists)
                {
                    // Read the file and display it line by line.
                    StreamWriter file = new StreamWriter(path);

                    file.Write(message);
                    file.Close();

                    TempData.Add("Message", "Fil opdateret");
                }
                else
                {
                    TempData.Add("Message", "File not found");

                }

            }

            return RedirectToAction("ReadFile");
        }
    }
}