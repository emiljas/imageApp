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
        [TestMethod]
        public void ConvertOneByte()
        {
            var expected = 0.000001;
            var result = CapacityConverter.ConvertByteToMegabytes(1);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ConvertToOneMegabytes()
        {
            var expected = 1;
            var result = CapacityConverter.ConvertByteToMegabytes(1000000);
            Assert.AreEqual(expected, result);
        }
    }
}
