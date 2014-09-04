using ImageApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace ImageApp.ImageEditorPage
{
    public sealed partial class ImageEditor : Page
    {
        private WriteableBitmap _editBitmap;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ImageEditor()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            var imageToEditPath = e.NavigationParameter as string;
            _editBitmap = await LoadWritableBitmap(imageToEditPath);
            EditImage.Source = _editBitmap;
            FitImageToCanvas();
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FitImageToCanvas();
        }

        private void FitImageToCanvas()
        {
            if (_editBitmap == null || EditImage.ActualWidth == 0)
                return;

            bool isImageBiggerThanPage = _editBitmap.PixelWidth > this.ActualWidth || _editBitmap.PixelHeight > this.ActualHeight;
            if(isImageBiggerThanPage)
            {
            }
            else
            {
                Canvas.SetLeft(EditImage, this.ActualWidth / 2 - EditImage.ActualWidth / 2);
            }
        }

        private void EditImage_Loaded(object sender, RoutedEventArgs e)
        {
            FitImageToCanvas();
        }

        private async Task<WriteableBitmap> LoadWritableBitmap(string path)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            var data = await FileIO.ReadBufferAsync(file);

            var ms = new InMemoryRandomAccessStream();
            var dw = new Windows.Storage.Streams.DataWriter(ms);
            dw.WriteBuffer(data);
            await dw.StoreAsync();
            ms.Seek(0);

            var bm = new BitmapImage();
            await bm.SetSourceAsync(ms);

            var wb = new WriteableBitmap(bm.PixelWidth, bm.PixelHeight);
            ms.Seek(0);

            await wb.SetSourceAsync(ms);

            return wb;
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Negative_Click(object sender, RoutedEventArgs e)
        {
            _editBitmap = _editBitmap.Invert();
            EditImage.Source = _editBitmap;
        }
        private void Brightness_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Crop_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EditImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            FitImageToCanvas();
        }

        private void EditImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FitImageToCanvas();
        }

        private void EditImage_Unloaded(object sender, RoutedEventArgs e)
        {
            FitImageToCanvas();
        }

        private void pageRoot_LayoutUpdated(object sender, object e)
        {
            FitImageToCanvas();
        }

        private void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            FitImageToCanvas();
        }

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FitImageToCanvas();
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            FitImageToCanvas();
        }

        private void Canvas_LayoutUpdated(object sender, object e)
        {
            FitImageToCanvas();
        }
    }
}
