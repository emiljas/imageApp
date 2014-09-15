using CSWindowsStoreAppCropBitmap;
using ImageApp.Common;
using ImageApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input;
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
        private WriteableBitmap editBitmap;
        private WriteableBitmap originalBitmap;
        private SelectedRegion selectedRegion;
        private WriteableBitmap beforeBrightnessCopy;

        private Dictionary<uint, Point?> pointerPositionHistory = new Dictionary<uint, Point?>();
        private string imageToEditPath;

        private uint sourceImagePixelWidth;
        private uint sourceImagePixelHeight;

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

        private Lazy<double> cornerSize = new Lazy<double>(delegate()
            {
                return (double)Application.Current.Resources["Size"];
            });

        public ImageEditor()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            selectRegion.ManipulationMode = ManipulationModes.Scale
                                            | ManipulationModes.TranslateX
                                            | ManipulationModes.TranslateY;
            selectedRegion = new SelectedRegion { MinSelectRegionSize = 2 * cornerSize.Value };
            this.DataContext = selectedRegion;
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {

            imageToEditPath = e.NavigationParameter as string;
            editBitmap = await LoadWritableBitmap(imageToEditPath);
            originalBitmap = editBitmap.Clone();
            sourceImage.Source = editBitmap;

            this.sourceImagePixelWidth = (uint)editBitmap.PixelWidth;
            this.sourceImagePixelHeight = (uint)editBitmap.PixelHeight;

            double sourceImageScale = 1;

            if (this.sourceImagePixelHeight < this.sourceImageGrid.ActualHeight &&
                this.sourceImagePixelWidth < this.sourceImageGrid.ActualWidth)
            {
                this.sourceImage.Stretch = Windows.UI.Xaml.Media.Stretch.None;
            }
            else
            {
                sourceImageScale = Math.Min(this.sourceImageGrid.ActualWidth / this.sourceImagePixelWidth,
                     this.sourceImageGrid.ActualHeight / this.sourceImagePixelHeight);
                this.sourceImage.Stretch = Windows.UI.Xaml.Media.Stretch.Uniform;
            }
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            selectedRegion.PropertyChanged += selectedRegion_PropertyChanged;

            AddCornerEvents(topLeftCorner);
            AddCornerEvents(topRightCorner);
            AddCornerEvents(bottomLeftCorner);
            AddCornerEvents(bottomRightCorner);

            selectRegion.ManipulationDelta += selectRegion_ManipulationDelta;
            selectRegion.ManipulationCompleted += selectRegion_ManipulationCompleted;

            this.sourceImage.SizeChanged += sourceImage_SizeChanged;
        }

        private void sourceImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.IsEmpty || double.IsNaN(e.NewSize.Height) || e.NewSize.Height <= 0)
            {
                this.imageCanvas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                this.selectedRegion.OuterRect = Rect.Empty;
                this.selectedRegion.ResetCorner(0, 0, 0, 0);
            }
            else
            {
                this.imageCanvas.Visibility = Windows.UI.Xaml.Visibility.Visible;

                this.imageCanvas.Height = e.NewSize.Height;
                this.imageCanvas.Width = e.NewSize.Width;
                this.selectedRegion.OuterRect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);

                if (e.PreviousSize.IsEmpty || double.IsNaN(e.PreviousSize.Height) || e.PreviousSize.Height <= 0)
                {
                    this.selectedRegion.ResetCorner(0, 0, e.NewSize.Width, e.NewSize.Height);
                }
                else
                {
                    double scale = e.NewSize.Height / e.PreviousSize.Height;
                    this.selectedRegion.ResizeSelectedRect(scale);
                }
            }
        }

        private void selectRegion_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.selectedRegion.UpdateSelectedRect(e.Delta.Scale, e.Delta.Translation.X, e.Delta.Translation.Y);
            e.Handled = true;
        }

        private void selectRegion_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
        }

        private void AddCornerEvents(Control corner)
        {
            corner.PointerPressed += corner_PointerPressed;
            corner.PointerMoved += corner_PointerMoved;
            corner.PointerReleased += corner_PointerReleased;
        }

        private void RemoveCornerEvents(Control corner)
        {
            corner.PointerPressed -= corner_PointerPressed;
            corner.PointerMoved -= corner_PointerMoved;
            corner.PointerReleased -= corner_PointerReleased;
        }

        private void corner_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as UIElement).CapturePointer(e.Pointer);
            PointerPoint point = e.GetCurrentPoint(this);
            pointerPositionHistory[point.PointerId] = point.Position;
            e.Handled = true;
        }

        private void corner_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint point = e.GetCurrentPoint(this);
            uint pointerId = point.PointerId;

            if (pointerPositionHistory.ContainsKey(pointerId) && pointerPositionHistory[pointerId].HasValue)
            {
                Point currentPosition = point.Position;
                Point previousPosition = pointerPositionHistory[pointerId].Value;

                double xUpdate = currentPosition.X - previousPosition.X;
                double yUpdate = currentPosition.Y - previousPosition.Y;

                this.selectedRegion.UpdateCorner((sender as ContentControl).Tag as string, xUpdate, yUpdate);

                pointerPositionHistory[pointerId] = currentPosition;
            }

            e.Handled = true;
        }

        private void corner_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            uint ptrId = e.GetCurrentPoint(this).PointerId;
            if (this.pointerPositionHistory.ContainsKey(ptrId))
            {
                this.pointerPositionHistory.Remove(ptrId);
            }

            (sender as UIElement).ReleasePointerCapture(e.Pointer);
            e.Handled = true;
        }

        void selectedRegion_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            double widthScale = imageCanvas.Width / this.sourceImagePixelWidth;
            double heightScale = imageCanvas.Height / this.sourceImagePixelHeight;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        private void Revert_Click(object sender, RoutedEventArgs e)
        {
            editBitmap = originalBitmap.Clone();
            sourceImage.Source = editBitmap;
            RevertButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
        }

        private void Negative_Click(object sender, RoutedEventArgs e)
        {
            editBitmap = editBitmap.Invert();
            sourceImage.Source = editBitmap;
            AfterEditAction();
        }

        private void Brightness_Click(object sender, RoutedEventArgs e)
        {
            beforeBrightnessCopy = editBitmap;
        }

        private void Brightness_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (beforeBrightnessCopy != null)
            {
                double amount = e.NewValue;
                editBitmap = beforeBrightnessCopy.Clone();
                editBitmap.Lighten(amount);
                sourceImage.Source = editBitmap;
                AfterEditAction();
            }
        }

        private void Crop_Click(object sender, RoutedEventArgs e)
        {
            double sourceImageWidthScale = imageCanvas.Width / this.sourceImagePixelWidth;
            double sourceImageHeightScale = imageCanvas.Height / this.sourceImagePixelHeight;
            var rect = new Rect
            {
                X = this.selectedRegion.SelectedRect.X / sourceImageWidthScale,
                Y = this.selectedRegion.SelectedRect.Y / sourceImageHeightScale,
                Width = this.selectedRegion.SelectedRect.Width / sourceImageWidthScale,
                Height = this.selectedRegion.SelectedRect.Height / sourceImageHeightScale
            };
            editBitmap = editBitmap.Crop(rect);
            sourceImage.Source = editBitmap;
            HideCropSelection();
            AfterEditAction();
        }

        private void AfterEditAction()
        {
            SaveButton.IsEnabled = true;
            RevertButton.IsEnabled = true;
        }

        private void HideCropSelection()
        {
            selectRegion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            nonselectRegion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            topLeftCorner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            topRightCorner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            bottomLeftCorner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            bottomRightCorner.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            editBitmap.SaveAsAsync(imageToEditPath);
            originalBitmap = editBitmap;
            editBitmap = originalBitmap.Clone();
            RevertButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.GoBack();
        }
    }
}
