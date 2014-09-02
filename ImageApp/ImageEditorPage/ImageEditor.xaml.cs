using ImageApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
            var file = await StorageFile.GetFileFromPathAsync(imageToEditPath);
            var bitmap = new BitmapImage();
            bitmap.SetSource(await file.OpenReadAsync());
            EditImage.Source = bitmap;


            var w = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);
            w.SetSource(await file.OpenReadAsync());
            var ww = w.Invert();
            EditImage.Source = ww;
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
    }
}
