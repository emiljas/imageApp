using ImageApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ImageApp.ImageBrowserPage
{
    public sealed partial class ImageBrowser : Page
    {
        public static readonly int LastPathsLimit = 10;

        private LastPathsManager lastPathManager = new LastPathsManager(LastPathsLimit);
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
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".gif");
            picker.FileTypeFilter.Add(".png");
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

        private void WritePath_Click(object sender, RoutedEventArgs e)
        {
            ResizeSetPathPopUp();
            LastPathsComboBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            SetPathPopUp.IsOpen = true;
            var lastPath = LastPathsComboBox.Items.FirstOrDefault();
            if(lastPath != null)
            {
                NewPathTextBox.Text = lastPath.ToString();
                NewPathTextBox.SelectAll();
                NewPathErrorTextBlock.Text = "";
            }
        }

        private void ResizeSetPathPopUp()
        {
            SetPathGrid.Width = LastPathsComboBox.ActualWidth;
            var relativePoint = CalculateSetPathPopUpPosition();
            SetPathPopUp.VerticalOffset = relativePoint.Y;
            SetPathPopUp.HorizontalOffset = relativePoint.X;
        }

        private Point CalculateSetPathPopUpPosition()
        {
            var parent = LastPathsComboBox.Parent as Grid;
            var transform = LastPathsComboBox.TransformToVisual(parent);
            var relativePoint = transform.TransformPoint(new Point(0, 0));
            return relativePoint;
        }

        private void SetPathPopUp_Closed(object sender, object e)
        {
            LastPathsComboBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void SetPathPopUp_Opened(object sender, object e)
        {
            NewPathTextBox.Focus(FocusState.Programmatic);
        }

        private void CancelSettingPath_Click(object sender, RoutedEventArgs e)
        {
            LastPathsComboBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
            SetPathPopUp.IsOpen = false;
        }

        private async void SetPath_Click(object sender, RoutedEventArgs e)
        {
            NewPathErrorTextBlock.Text = "";
            var path = NewPathTextBox.Text;
            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(path);
            try
            {
                var folder = await StorageFolder.GetFolderFromPathAsync(path);
                lastPathManager.Add(path);
            }
            catch(Exception ex)
            {
                NewPathErrorTextBlock.Text = ex.Message;
            }
        }
    }
}
