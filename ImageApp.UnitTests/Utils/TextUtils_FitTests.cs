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
    public class TextUtils_FitTests
    {
        private const int Limit = 5;

        [TestMethod]
        public void IfTextIsShorterThanLimitThenIsNotChanged()
        {
            const string text = "abc";
            var result = TextUtils.Fit(text, Limit);
            Assert.AreEqual(text, result);
        }

        [TestMethod]
        public void IfTextIsLongerThanLimitThenResultIsEndedThreeDots()
        {
            const string text = "abcdefghijk";
            const string expected = "ab...";
            var result = TextUtils.Fit(text, Limit);
            Assert.AreEqual(expected, result);
        }
    }
}
