using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.Utils
{
    public class PathUtils
    {
        public SplittedPath Split(string path)
        {
            var sp = new SplittedPath();
            var index = path.LastIndexOf(@"\");
            sp.Directory = path.Substring(0, index);
            sp.FileNameWithExtension = path.Substring(index + 1);
            sp.FileNameWithoutExtension = RemoveExtension(sp.FileNameWithExtension);
            return sp;
        }

        private string RemoveExtension(string fileNameWithExtension)
        {
            int dotIndex = fileNameWithExtension.LastIndexOf(".");
            return fileNameWithExtension.Substring(0, dotIndex);
        }

        public string Join(string directory, string fileName)
        {
            return Path.Combine(directory, fileName);
        }

        public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }
    }

    public class SplittedPath
    {
        public string Directory { get; set; }

        public string FileNameWithExtension { get; set; }

        public string FileNameWithoutExtension { get; set; }
    }
}
