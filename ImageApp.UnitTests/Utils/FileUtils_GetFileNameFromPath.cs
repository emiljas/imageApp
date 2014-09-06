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
    public class FileUtils_Split
    {
		[TestMethod]
		public void SplitCorectPath()
        {
			var fileUtils = new FileUtils();
			var path = @"C:\Users\Emil\Pictures\got\abc.png";
			var result = fileUtils.Split(path);
			Assert.AreEqual(@"C:\Users\Emil\Pictures\got", result.Directory);
			Assert.AreEqual("abc.png", result.FileName);
        }
    }
}
