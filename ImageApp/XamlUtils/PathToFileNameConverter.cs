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
        private PathUtils pathUtils = new PathUtils();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var path = (string)value;
            return pathUtils.Split(path).FileNameWithExtension;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
