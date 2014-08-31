using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ImageApp.ImageBrowserPage;

namespace ImageApp.UnitTests.ImageBrowser
{
    [TestClass]
    public class LastPathsManagerTests
    {
        private const int Limit = 3;

        [TestMethod]
        public void NoPathsOnFreshStart()
        {
            var manager = MakeManager();
            Assert.AreEqual(0, manager.Paths.Count);
        }

        [TestMethod]
        public void NewPathIsOnTop()
        {
            const string newPath = "3";
            var manager = MakeManager();
            manager.Paths = new List<string>
            {
                "2",
                "1"
            };

            manager.Add(newPath);

            Assert.AreEqual(newPath, manager.Paths[0]);
        }

        [TestMethod]
        public void OldestPathIsRemoveIfLimitIsExceed()
        {
            var manager = MakeManager();

            manager.Add("1");
            manager.Add("2");
            manager.Add("3");
            manager.Add("4");

            Assert.AreEqual("4", manager.Paths.First());
            Assert.AreEqual("2", manager.Paths.Last());
            Assert.AreEqual(Limit, manager.Paths.Count);
        }

        [TestMethod]
        public void DuplicatesAreMergedToOnePosition()
        {
            var manager = MakeManager();

            manager.Add("1");
            manager.Add("2");
            manager.Add("1");

            Assert.AreEqual("1", manager.Paths[0]);
            Assert.AreEqual("2", manager.Paths[1]);
            Assert.AreEqual(2, manager.Paths.Count);
        }

        private LastPathsManager MakeManager()
        {
            return new LastPathsManager(Limit);
        }
    }
}
