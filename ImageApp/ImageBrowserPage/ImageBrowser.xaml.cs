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
        private ImageBrowserViewModel viewModel;
        private PathUtils pathUtils = new PathUtils();

        public ImageBrowser()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            App.Current.Suspending += App_Suspending;
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
            LoadState();
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            SaveState();
        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            SaveState();
        }

        private void LoadState()
        {
            viewModel = Settings.ViewModel;
            this.DataContext = this.viewModel;

            lastPathManager.Paths = new List<string>(viewModel.LastPaths);
        }

        private void SaveState()
        {
            Settings.ViewModel = viewModel;
        }

        private async void PickFolder_Click(object sender, RoutedEventArgs e)
        {
            var picker = this.MakeFolderPicker();
            var folder = await picker.PickSingleFolderAsync();
            StorageApplicationPermissions.FutureAccessList.Add(folder);
            this.UpdateLastPaths(folder.Path);
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
            this.SelectFirstPath();
        }

        private void UpdateLastPaths()
        {
            this.viewModel.LastPaths = new List<string>(this.lastPathManager.Paths);
        }

        private void SelectFirstPath()
        {
            this.viewModel.SelectedPathIndex = 0;
        }

        private async void LastPathsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await UpdateThumbnails();
        }

        private async System.Threading.Tasks.Task UpdateThumbnails()
        {
            if (this.viewModel.LastPaths.Count != 0 && this.viewModel.SelectedPathIndex != -1)
            {
                if (this.path != this.viewModel.SelectedPath)
                    await ForceUpdateThumbnail();
            }
        }

        private async System.Threading.Tasks.Task ForceUpdateThumbnail()
        {
            ThumbnailsListView.Items.Clear();
            this.path = this.viewModel.SelectedPath;
            var folder = await StorageFolder.GetFolderFromPathAsync(this.path);
            var files = await folder.GetFilesAsync();

            foreach (var file in files)
            {
                if (SupportedImages.IsSupported(file.FileType))
                {
                    var thumbnail = new Thumbnail();
                    thumbnail.Click += this.Thumbnail_Click;
                    thumbnail.SetFileAsync(file);
                    ThumbnailsListView.Items.Add(thumbnail);
                }
            }
        }

        private async void Thumbnail_Click(object sender, EventArgs e)
        {
            var thumbnail = sender as Thumbnail;
            var file = thumbnail.File;
            var bitmap = new BitmapImage();
            var stream = await file.OpenReadAsync();
            await bitmap.SetSourceAsync(stream);
            viewModel.SelectedImagePath = file.Path;


            viewModel.FileName = file.Name;
            var properties = await file.GetBasicPropertiesAsync();
            viewModel.Size = CapacityConverter.ConvertByteToMegabytes(properties.Size) + " MB";
            viewModel.LastModifiedDate = properties.DateModified.LocalDateTime.ToString();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ImageEditor), viewModel.SelectedImagePath);
        }

        private async void Rename_ClickAsync(object sender, RoutedEventArgs e)
        {
            var directory = pathUtils.Split(viewModel.SelectedImagePath).Directory;
            var fileName = RenameTextBox.Text + pathUtils.GetExtension(viewModel.SelectedImagePath);
            var file = await StorageFile.GetFileFromPathAsync(viewModel.SelectedImagePath);
            await file.RenameAsync(fileName, NameCollisionOption.GenerateUniqueName);
            await ForceUpdateThumbnail();
            RenameFlyout.Hide();
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var file = await StorageFile.GetFileFromPathAsync(viewModel.SelectedImagePath);
            await file.DeleteAsync();
            await ForceUpdateThumbnail();
            DeleteFlyout.Hide();
        }

    }
}
