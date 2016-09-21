using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SystemIoTasks.Models
{
    public class ReadFileViewModel
    {
        public string Message { get; set; }
        public string Name { get; set; }
        public List<FileInfo> Files { get; set; }

        public ReadFileViewModel()
        {
            Files = new List<FileInfo>();
        }
    }
}