using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemIoTasks.Controllers
{
    public class FoldersController : Controller
    {
        // GET: Folders
        public ActionResult Index()
        {

            List<string> folderPaths = Directory.GetDirectories(Server.MapPath("~/Data")).ToList();
            List<DirectoryInfo> folders = new List<DirectoryInfo>();
            foreach (string folder in folderPaths)
            {
                folders.Add(new DirectoryInfo(folder));
            }

            return View(folders);
        }

        [HttpPost]
        public ActionResult Index(string Name)
        {

            string path = Server.MapPath("~/Data/" + Name);
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                TempData.Add("Message", "Mappe Oprettet");
            }else
            {
                TempData.Add("Message", $"Mappen med navnet {Name} eksistere allerede");
            }

            List<string> folderPaths = Directory.GetDirectories(Server.MapPath("~/Data")).ToList();
            List<DirectoryInfo> folders = new List<DirectoryInfo>();
            foreach(string folder in folderPaths)
            {
                folders.Add(new DirectoryInfo(folder));
            }

            return View(folders);
        }
    }
}