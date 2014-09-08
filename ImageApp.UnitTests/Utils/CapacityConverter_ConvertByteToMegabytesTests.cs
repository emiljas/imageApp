using ImageApp.Utils;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.UnitTests.Utils
{
    [TestClass]
    public class CapacityConverter_ConvertByteToMegabytesTests
    {
        private const double Delta = 0.01;

        [TestMethod]
        public void Convert()
        {
            ulong bytes = 614940;
            double megabytes = 0.58645;
            var result = CapacityConverter.ConvertByteToMegabytes(bytes);
            Assert.AreEqual(megabytes, result, Delta);
        }
    }
}
