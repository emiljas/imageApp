using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.Data
{
    public static class SupportedImages
    {
        public static readonly string[] Extensions = new string[]
        {
            ".jpg",
            ".jpeg",
            ".gif",
            ".png"
        };

        public static bool IsSupported(string extension)
        {
            return Extensions.Contains(extension.ToLower());
        }
    }
}
