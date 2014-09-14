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
    public class PathUtils_GetExtension
    {
        [TestMethod]
        public void GetExtension()
        {
            var path = @"c:\folder\obraz.jpg";
            var pathUtils = new PathUtils();
            var extension = pathUtils.GetExtension(path);
            Assert.AreEqual(".jpg", extension);
        }
    }
}
