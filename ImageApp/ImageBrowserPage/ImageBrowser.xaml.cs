namespace ImageApp.ImageBrowserPage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using ImageApp.Common;
    using ImageApp.Data;
    using ImageApp.ImageEditorPage;
    using ImageApp.Utils;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.Storage;
    using Windows.Storage.AccessCache;
    using Windows.Storage.Pickers;
    using Windows.UI.Core;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    public sealed partial class ImageBrowser : Page
    {
        public static readonly int LastPathsLimit = 10;
        public static readonly ImageBrowserSettings Settings = new ImageBrowserSettings();

        private LastPathsManager lastPathManager = new LastPathsManager(LastPathsLimit);
        private NavigationHelper navigationHelper;
        private string path;
        private string selectedImagePath;
        private ImageBrowserViewModel viewModel;

        public ImageBrowser()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            viewModel = Settings.ViewModel; 
            this.DataContext = this.viewModel;

            //this.lastPathManager.Paths = Settings.LastPaths;
            //this.lastPathManager.Paths = viewModel.LastPaths;
            //this.UpdateLastPaths();
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            Settings.ViewModel = viewModel;
            //Settings.LastPaths = this.lastPathManager.Paths;
        }

        private async void PickFolder_Click(object sender, RoutedEventArgs e)
        {
            var picker = this.MakeFolderPicker();
            var folder = await picker.PickSingleFolderAsync();
            StorageApplicationPermissions.FutureAccessList.Add(folder);
            this.UpdateLastPaths(folder.Path);
            //this.SelectFirstPath();
        }

        private FolderPicker MakeFolderPicker()
        {
            var picker = new FolderPicker();
            this.FilterPictureExtensions(picker);
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            return picker;
        }

        private void FilterPictureExtensions(FolderPicker picker)
        {
            foreach (var fileType in SupportedImages.Extensions)
                picker.FileTypeFilter.Add(fileType);
        }

        private void UpdateLastPaths(string newPath)
        {
            this.lastPathManager.Add(newPath);
            this.UpdateLastPaths();
        }

        private void UpdateLastPaths()
        {
            this.viewModel.LastPaths = new List<string>(this.lastPathManager.Paths);
            //LastPathsComboBox.Items.Clear();
            //foreach (var path in this.lastPathManager.Paths)
            //    LastPathsComboBox.Items.Add(path);
        }

        //private void SelectFirstPath()
        //{
        //    this.viewModel.SelectedPathIndex = 0;
        //}

        private async void LastPathsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (this.viewModel.LastPaths.Count != 0)
            //{
            //    ThumbnailsListView.Items.Clear();

            //    var selectedPath = this.viewModel.SelectedPath;
            //    if (this.path != selectedPath)
            //    {
            //        this.path = selectedPath;
            //        var folder = await StorageFolder.GetFolderFromPathAsync(this.path);
            //        var files = await folder.GetFilesAsync();

            //        foreach (var file in files)
            //        {
            //            if (SupportedImages.IsSupported(file.FileType))
            //            {
            //                var thumbnail = new Thumbnail();
            //                thumbnail.Click += this.Thumbnail_Click;
            //                thumbnail.SetFileAsync(file);
            //                ThumbnailsListView.Items.Add(thumbnail);
            //            }
            //        }
            //    }
            //}
        }

        private async void Thumbnail_Click(object sender, EventArgs e)
        {
            //BottomCommandAppBar.IsEnabled = true;
            //var thumbnail = sender as Thumbnail;
            //var file = thumbnail.File;
            //var bitmap = new BitmapImage();
            //var stream = await file.OpenReadAsync();
            //await bitmap.SetSourceAsync(stream);
            //SelectedImage.Source = bitmap;

            //this.selectedImagePath = file.Path;

            //viewModel.FileName = file.Name;
            //var properties = await file.GetBasicPropertiesAsync();
            //viewModel.Size = CapacityConverter.ConvertByteToMegabytes(properties.Size) + " MB";
            //viewModel.LastModifiedDate = properties.DateModified.LocalDateTime.ToString();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper_SaveState(this, SaveStateEventArgs.Empty as SaveStateEventArgs);
            //this.Frame.Navigate(typeof(ImageEditor), this.selectedImagePath);
        }
    }
}
