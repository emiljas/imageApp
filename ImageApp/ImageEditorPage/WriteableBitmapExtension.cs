﻿using ImageApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageApp.ImageEditorPage
{
    public static class WriteableBitmapExtension
    {
        private static PathUtils pathUtils = new PathUtils();

        /// <summary>
        /// Lightens the specified bitmap.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="amount">The 0..1 range amount to lighten by where 0 does not affect the bitmap and 1 makes the bitmap completely white.</param>
        /// <returns></returns>
        public static void Lighten(this WriteableBitmap target, double amount)
        {
            var bytes = target.ToByteArray();

            for (int i = 0; i < bytes.Length; i += 4)
            {
                byte a = bytes[i + 3];

                if (a > 0)
                {
                    double ad = (double)a / 255.0; // 0..1 range alpha
                    double rd = (double)bytes[i + 2] / ad; // 0..255 range red, non-alpha-premultiplied
                    double gd = (double)bytes[i + 1] / ad; // 0..255 range green, non-alpha-premultiplied
                    double bd = (double)bytes[i + 0] / ad; // 0..255 range blue, non-alpha-premultiplied

                    // gain is the difference between current value and maximum (255), multiplied by the amount and alpha-premultiplied
                    double gainR = (255.0 - rd) * amount * ad;
                    double gainG = (255.0 - gd) * amount * ad;
                    double gainB = (255.0 - bd) * amount * ad;

                    bytes[i + 0] += (byte)gainB;
                    bytes[i + 1] += (byte)gainG;
                    bytes[i + 2] += (byte)gainR;
                }
            }

            target.FromByteArray(bytes);
        }

        public static async void SaveAsAsync(this WriteableBitmap wb, string path)
        {
            var splittedPath = pathUtils.Split(path);
            var folder = await StorageFolder.GetFolderFromPathAsync(splittedPath.Directory);
            var file = await folder.CreateFileAsync(splittedPath.FileNameWithExtension, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                var pixels = wb.ToByteArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)wb.PixelWidth, (uint)wb.PixelHeight, 96.0, 96.0, pixels);
                await encoder.FlushAsync();
            }
        }
    }
}
