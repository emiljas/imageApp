using ImageApp.ImageBrowserPage;
using ImageApp.UnitTests.Mock;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.UnitTests.ImageBrowserPage
{
    [TestClass]
    public class ImageBrowserSettingsTests
    {
        private const string Path = "a\\b";

        [TestMethod]
        public void StoreAndRetriveViewModel()
        {
            var settings = MakeSettings();
            var viewModel = MakeViewModelWithPath();
            settings.ViewModel = viewModel;
            var retrivedViewModel = settings.ViewModel;
            Assert.AreEqual(Path, retrivedViewModel.LastPaths[0]);
        }

        private ImageBrowserSettings MakeSettings()
        {
            var settings = new ImageBrowserSettings();
            settings.InjectSettings(new PropertySetMock());
            return settings;
        }

        private ImageBrowserViewModel MakeViewModelWithPath()
        {
            var viewModel = new ImageBrowserViewModel();
            viewModel.LastPaths = new List<string>
            {
                Path
            };
            return viewModel;
        }
    }
}
