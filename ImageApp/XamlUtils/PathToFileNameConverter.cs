using ImageApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ImageApp.XamlUtils
{
    public class PathToFileNameConverter : IValueConverter
    {
        private FileUtils fileUtils = new FileUtils();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = (string)value;
            return fileUtils.Split(path).FileName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
