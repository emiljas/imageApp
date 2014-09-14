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
    public class PathUtils_Join
    {
        [TestMethod]
        public void Join()
        {
            string directory = @"C:\Users\Emil\Pictures\got";
            string fileName = "abc.jpg";
            var pathUtils = new PathUtils();
            var path = pathUtils.Join(directory, fileName);
            Assert.AreEqual(@"C:\Users\Emil\Pictures\got\abc.jpg", path);
        }
    }
}
