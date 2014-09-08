using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.ImageBrowserPage
{
    public class ImageBrowserViewModel : INotifyPropertyChanged
    {
        private List<string> lastPaths;
        public List<string> LastPaths
        {
            get
            {
                return lastPaths ?? new List<string>();
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
