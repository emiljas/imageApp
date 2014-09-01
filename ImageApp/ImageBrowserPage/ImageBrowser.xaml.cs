using ImageApp.Common;
using ImageApp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace ImageApp.ImageBrowserPage
{
    public sealed partial class ImageBrowser : Page
    {
        public static readonly int LastPathsLimit = 10;

        private LastPathsManager lastPathManager = new LastPathsManager(LastPathsLimit);
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string path;

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public ImageBrowser()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
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

        private async void PickFolder_Click(object sender, RoutedEventArgs e)
        {
            var picker = MakeFolderPicker();
            var folder = await picker.PickSingleFolderAsync();
            StorageApplicationPermissions.FutureAccessList.Add(folder);
            UpdateLastPaths(folder.Path);
            SelectFirstPath();
        }

        private FolderPicker MakeFolderPicker()
        {
            var picker = new FolderPicker();
            FilterPictureExtensions(picker);
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            return picker;
        }

        private static void FilterPictureExtensions(FolderPicker picker)
        {
            foreach (var fileType in SupportedImages.Extensions)
                picker.FileTypeFilter.Add(fileType);
        }

        private void UpdateLastPaths(string newPath)
        {
            lastPathManager.Add(newPath);
            LastPathsComboBox.Items.Clear();
            foreach(var path in lastPathManager.Paths)
                LastPathsComboBox.Items.Add(path);
        }

        private void SelectFirstPath()
        {
            LastPathsComboBox.SelectedIndex = 0;
        }

        private async void LastPathsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPath = LastPathsComboBox.Items.First().ToString();
            if(this.path != selectedPath)
            {
                this.path = selectedPath;
                var folder = await StorageFolder.GetFolderFromPathAsync(this.path);
                var files = await folder.GetFilesAsync();

                foreach (var file in files)
                {
                    if (SupportedImages.IsSupported(file.FileType))
                    {
                        var thumbnail = new Thumbnail();
                        thumbnail.SetFileAsync(file);
                        ThumbnailsStackPanel.Children.Add(thumbnail);
                    }
                }
            }
        }
    }
}
