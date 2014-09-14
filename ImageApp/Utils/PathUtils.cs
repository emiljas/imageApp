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
            sp.FileName = path.Substring(index + 1);
            return sp;
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
        public string FileName { get; set; }
    }
}
