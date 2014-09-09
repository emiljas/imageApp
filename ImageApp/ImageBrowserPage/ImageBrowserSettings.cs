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
        //private readonly string lastPathsKey;
        //private readonly string selectedDirectoryKey;

        public ImageBrowserSettings()
        {
            this.BaseKey = "ImageBrowser";
            this.viewModelKey = MakeKey("ViewModel");
            //this.lastPathsKey = this.MakeKey("LastPathsKey");
            //this.selectedDirectoryKey = this.MakeKey("SelectedDirectory");
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

        //public List<string> LastPaths
        //{
        //    get
        //    {
        //        string serializedValue = base[this.lastPathsKey] as string ?? string.Empty;
        //        var lastPaths = JsonConvert.DeserializeObject<List<string>>(serializedValue);
        //        return lastPaths ?? new List<string>();
        //    }

        //    set
        //    {
        //        string serializedValue = JsonConvert.SerializeObject(value);
        //        base[this.lastPathsKey] = serializedValue;
        //    }
        //}

        //public string SelectedDirectory
        //{
        //    get { return base[this.selectedDirectoryKey] as string; }

        //    set { base[this.selectedDirectoryKey] = value; }
        //}

        //public bool ExistsSelectedDirectorySetting()
        //{
        //    return this.Exists(this.selectedDirectoryKey);
        //}
    }
}
