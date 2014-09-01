using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.Utils
{
    public static class CapacityConverter
    {
        public const double OneByteInMegabytes = 0.000001;

        public static double ConvertByteToMegabytes(ulong bytes)
        {
            return bytes * OneByteInMegabytes;
        }
    }
}
