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
    public class PathUtils_Split
    {
		[TestMethod]
		public void Split()
        {
			var pathUtils = new PathUtils();
			var path = @"C:\Users\Emil\Pictures\got\abc.png";
			var result = pathUtils.Split(path);
			Assert.AreEqual(@"C:\Users\Emil\Pictures\got", result.Directory);
			Assert.AreEqual("abc.png", result.FileNameWithExtension);
            Assert.AreEqual("abc", result.FileNameWithoutExtension);
        }
    }
}
