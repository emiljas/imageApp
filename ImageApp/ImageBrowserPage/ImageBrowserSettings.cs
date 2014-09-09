namespace ImageApp.ImageBrowserPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ImageApp.Utils;
    using Newtonsoft.Json;
    using Windows.Foundation.Collections;

    public class ImageBrowserSettings : LocalSettings
    {
        private readonly string viewModelKey;

        public ImageBrowserSettings()
        {
            this.BaseKey = "ImageBrowser";
            this.viewModelKey = MakeKey("ViewModel");
        }

        public ImageBrowserViewModel ViewModel
        {
            get
            {
                var serializedViewModel = this[this.viewModelKey] as string;
                if (string.IsNullOrEmpty(serializedViewModel))
                    return new ImageBrowserViewModel();
                return JsonConvert.DeserializeObject<ImageBrowserViewModel>(serializedViewModel);
            }

            set
            {
                var serializedViewModel = JsonConvert.SerializeObject(value);
                this[this.viewModelKey] = serializedViewModel;
            }
        }
    }
}
