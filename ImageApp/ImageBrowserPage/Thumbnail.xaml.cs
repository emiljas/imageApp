using ImageApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ImageApp.ImageBrowserPage
{
    public sealed partial class Thumbnail : UserControl
    {
        private StorageFile file;
        private const int Limit = 30;

        public StorageFile File
        {
            get { return file; }
        }

        public event EventHandler Click;

        public Thumbnail()
        {
            this.InitializeComponent();
        }

        public async void SetFileAsync(StorageFile file)
        {
            this.file = file;
            await LoadThumbnail(file);
            ShowDescription(file.DisplayName);
        }

        private void ShowDescription(string displayName)
        {
            DescriptionTextBlock.Text = TextUtils.Fit(displayName, Limit);
        }

        private async Task LoadThumbnail(StorageFile file)
        {
            var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.PicturesView);
            var stream = thumbnail.CloneStream();
            var bitmap = new BitmapImage();
            await bitmap.SetSourceAsync(stream);
            ThumbnailImage.Source = bitmap;
        }

        //private void StackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        //{
        //    StackPanel.Background = new SolidColorBrush(Colors.LightGray);
        //    StackPanel.Background.Opacity = 0.3;
        //}

        //private void StackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        //{
        //    StackPanel.Background = new SolidColorBrush(Colors.Black);
        //    StackPanel.Background.Opacity = 1;
        //}

        private void StackPanel_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (Click != null)
                Click(this, EventArgs.Empty);
        }
    }
}
