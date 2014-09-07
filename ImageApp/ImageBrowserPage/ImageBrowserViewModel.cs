using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageApp.ImageBrowserPage
{
    public class ImageBrowserViewModel : INotifyPropertyChanged
    {
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; Notify("FileName"); }
        }

        private string size;
        public string Size
        {
            get { return size; }
            set { size = value; Notify("Size"); }
        }

        private string lastModifiedDate;
        public string LastModifiedDate
        {
            get { return lastModifiedDate; }
            set { lastModifiedDate = value; Notify("LastModifiedDate"); }
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
