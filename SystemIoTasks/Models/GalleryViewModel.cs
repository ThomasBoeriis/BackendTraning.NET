using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;

namespace SystemIoTasks.Models
{
    public class GalleryViewModel
    {
        public List<DirectoryInfo> RootFolders { get; set; }
        public string CurrentFolder { get; set; }
        public List<FileInfo> FilesInCurrentFolder { get; set; }

        public GalleryViewModel()
        {
            RootFolders = new List<DirectoryInfo>();
        }
    }
}