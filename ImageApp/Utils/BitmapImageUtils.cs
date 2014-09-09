using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageApp.Utils
{
    public class BitmapImageUtils
    {
        public static async Task<BitmapImage> LoadAsync(string path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            var bitmap = new BitmapImage();
            var stream = await file.OpenReadAsync();
            await bitmap.SetSourceAsync(stream);
            return bitmap;
        }
    }
}
