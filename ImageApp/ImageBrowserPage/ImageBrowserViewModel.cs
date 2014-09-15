using ImageApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageApp.ImageBrowserPage
{
    public class ImageBrowserViewModel : INotifyPropertyChanged
    {
        private List<string> lastPaths;
        public List<string> LastPaths
        {
            get
            {
                if (lastPaths == null)
                    lastPaths = new List<string>();
                return lastPaths;
            }

            set 
            {
                lastPaths = value;
                Notify("LastPaths");
            }
        }

        private int selectedPathIndex;
        public int SelectedPathIndex
        {
            get
            {
                return selectedPathIndex;
            }

            set
            {
                selectedPathIndex = value;
                Notify("SelectedPathIndex");
            }
        }

        public string SelectedPath
        {
            get
            {
                return lastPaths[selectedPathIndex];
            }
        }

        private string selectedImagePath;
        public string SelectedImagePath
        {
            get
            {
                return selectedImagePath;
            }

            set
            {
                selectedImagePath = value;
                Notify("SelectedImagePath");
                if(!string.IsNullOrEmpty(selectedImagePath))
                {
                    var loadingTask = BitmapImageUtils.LoadAsync(selectedImagePath);
                    loadingTask.ContinueWith((Task<BitmapImage> task) =>
                    {
                        CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(delegate()
                        {
                            SelectedImage = task.Result;
                        }));

                    });
                }
            }
        }

        private BitmapImage selectedImage;
        [JsonIgnore]
        public BitmapImage SelectedImage
        {
            get
            {
                return selectedImage;
            }

            set
            {
                selectedImage = value;
                Notify("SelectedImage");
            }
        }
        

        private string fileName;
        public string FileName
        {
            get 
            {
                return fileName ?? string.Empty;
            }

            set
            {
                fileName = value; 
                Notify("FileName");
            }
        }

        private string size;
        public string Size
        {
            get
            {
                return size ?? string.Empty;
            }

            set
            {
                size = value;
                Notify("Size"); 
            }
        }

        private string lastModifiedDate;
        public string LastModifiedDate
        {
            get
            {
                return lastModifiedDate ?? string.Empty; 
            }

            set 
            {
                lastModifiedDate = value; 
                Notify("LastModifiedDate");
            }
        }

        private void Notify(string propertyName)
        {
            if(PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
