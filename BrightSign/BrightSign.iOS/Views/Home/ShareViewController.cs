using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Utility.Interface;
using BrightSign.iOS.Views.CustomViews;
using BrightSign.Localization;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;

namespace BrightSign.iOS.Views.Home
{
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ShareViewController : BaseView<ShareViewModel>, IUIScrollViewDelegate
    {
        CGSize imageSize;

        UIImageView previousImageView;
        UIScrollView previousScrollView;
        float currentTransform;

        UIImageView currentImageView;
        UIScrollView currentScrollView;


        public ShareViewController() : base("ShareViewController", null, false)
        {

        }

        //public override bool ShouldAutorotate()
        //{
        //    return true;
        //}

        //public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        //{
        //    return UIInterfaceOrientationMask.All;
        //}

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var backButton = new UIBarButtonItem(UIImage.FromBundle("ic_chevron_left_white.png"), UIBarButtonItemStyle.Plain, BackBarButtonItemAction);
            NavigationItem.LeftBarButtonItem = backButton;


            UIButton leftrotateButton = UIButton.FromType(UIButtonType.Custom);
            leftrotateButton.SetImage(UIImage.FromFile("baseline_rotate_left_white.png"), UIControlState.Normal);
            leftrotateButton.Frame = new CGRect(0, 0, 30, 30);
            leftrotateButton.TouchUpInside -= LeftToolBarButton_Clicked;

            leftrotateButton.TouchUpInside += LeftToolBarButton_Clicked;
            //UIBarButtonItem leftToolBarButton = new UIBarButtonItem(UIImage.FromFile("baseline_rotate_left_white.png"), UIBarButtonItemStyle.Plain, LeftToolBarButton_Clicked);
            //leftToolBarButton.ImageInsets = new UIEdgeInsets(0, 75, 0, 0);
            //leftToolBarButton.ImageInsets = new UIEdgeInsets(0, 85, 0, 0);
            UIBarButtonItem leftToolBarButton = new UIBarButtonItem(leftrotateButton);


            UIButton rightrotateButton = UIButton.FromType(UIButtonType.Custom);
            rightrotateButton.SetImage(UIImage.FromFile("baseline_rotate_right_white.png"), UIControlState.Normal);
            rightrotateButton.Frame = new CGRect(0, 0, 30, 30);
            rightrotateButton.TouchUpInside -= RightToolbarButton_Clicked;

            rightrotateButton.TouchUpInside += RightToolbarButton_Clicked;
            UIBarButtonItem rightToolbarButton = new UIBarButtonItem(rightrotateButton);

            //UIBarButtonItem rightToolbarButton = new UIBarButtonItem(UIImage.FromFile("baseline_rotate_right_white.png"), UIBarButtonItemStyle.Plain, RightToolbarButton_Clicked);
            //rightToolbarButton.ImageInsets = new UIEdgeInsets(0, 10, 0, -20);
            //rightToolbarButton.ImageInsets = new UIEdgeInsets(0, 40, 0, 0);

            UIButton sharebButton = UIButton.FromType(UIButtonType.Custom);
            sharebButton.SetImage(UIImage.FromFile("baseline_share_white.png"), UIControlState.Normal);
            sharebButton.Frame = new CGRect(0, 0, 30, 30);
            sharebButton.TouchUpInside -= actionOnShareBtn;

            sharebButton.TouchUpInside += actionOnShareBtn;
            UIBarButtonItem shareButton = new UIBarButtonItem(sharebButton);

            //var shareButton = new UIBarButtonItem(UIImage.FromFile("baseline_share_white.png"), UIBarButtonItemStyle.Plain, actionOnShareBtn);
            //shareButton.ImageInsets = new UIEdgeInsets(0, -30, 0, 0);

            //NavigationItem.RightBarButtonItem = shareButton;

            UIBarButtonItem[] items = new UIBarButtonItem[3] { shareButton, rightToolbarButton, leftToolBarButton };
            NavigationItem.RightBarButtonItems = items;



            imageView.UserInteractionEnabled = true;
            imageView.AutoresizingMask = UIViewAutoresizing.All;


            //shareBtn.Clicked += actionOnShareBtn;
            //leftRotateBtn.Clicked += actionOnLeftRotateBtn;
            //rightRotateBtn.Clicked += actionOnRightRotateBtn;
            ((MvxNotifyPropertyChanged)this.ViewModel).PropertyChanged += OnPropertyChanged;
            loadUIData();
            // TAP GESTURE
            UITapGestureRecognizer doubletap = new UITapGestureRecognizer(OnDoubleTap)
            {
                NumberOfTapsRequired = 2 //NUMBER OF TAPS
            };
            //scrollView.AddGestureRecognizer(doubletap);

            // TAP GESTURE
            UITapGestureRecognizer singleTap = new UITapGestureRecognizer(OnSingleTap)
            {
                NumberOfTapsRequired = 1 //NUMBER OF TAPS
            };
            scrollView.AddGestureRecognizer(singleTap);

            UISwipeGestureRecognizer rightSwipe = new UISwipeGestureRecognizer(Swipe);
            rightSwipe.Direction = UISwipeGestureRecognizerDirection.Right;

            UISwipeGestureRecognizer leftSwipe = new UISwipeGestureRecognizer(Swipe);
            leftSwipe.Direction = UISwipeGestureRecognizerDirection.Left;

            //scrollView.AddGestureRecognizer(rightSwipe);
            //scrollView.AddGestureRecognizer(leftSwipe);

            scrollView.DidZoom += (Object sender, EventArgs e) =>
            {


                CenterImageInScrollView();
            };

            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;


            /* New Way of implementing the image sliding 

            imageSize = new CGSize(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 80);

            // Perform any additional setup after loading the view, typically from a nib.

            scrollView.Frame = new CGRect(10, 10, imageSize.Width, imageSize.Height);
            scrollView.ContentSize = new CGSize(imageSize.Width * Constants.BSSnapshotList.Count, imageSize.Height);
            scrollView.PagingEnabled = true;

            double xPos = 0.0;
            foreach (var item in Constants.BSSnapshotList)
            {
                UIImageView imageView = new UIImageView();
                imageView.Frame = new CGRect(0.0, 0.0, imageSize.Width, imageSize.Height);
                imageView.Image = FromUrl(item.ImageUrl);
                imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

                UIScrollView imageScrollView = new UIScrollView();
                imageScrollView.Frame = new CGRect(xPos, 0.0, imageSize.Width, imageSize.Height);
                imageScrollView.MinimumZoomScale = 1.0f;
                imageScrollView.MaximumZoomScale = 2.0f;
                imageScrollView.ZoomScale = 1.0f;
                imageScrollView.ContentSize = imageView.Bounds.Size;


                imageScrollView.ViewForZoomingInScrollView += (scrollView) =>
                {

                    return scrollView.Subviews[0];
                };
                imageScrollView.ScrollEnabled = true;
                imageScrollView.MinimumZoomScale = 1;
                imageScrollView.MaximumZoomScale = 100;
                UITapGestureRecognizer doubletap = new UITapGestureRecognizer(() =>
                {
                    if (imageScrollView.ZoomScale >= 5)
                    {
                        imageScrollView.SetZoomScale(1f, true);

                    }
                    else
                    {
                        imageScrollView.SetZoomScale(5, true);

                    }
                });
                doubletap.NumberOfTapsRequired = 2; //NUMBER OF TAPS
                imageScrollView.AddGestureRecognizer(doubletap);

                imageScrollView.AddSubview(imageView);

                scrollView.AddSubview(imageScrollView);
                xPos += imageSize.Width;
            }

            selectImage(ViewModel.selectedIndex);

            var size = scrollView.ContentSize;

            scrollView.Scrolled += (object sender, EventArgs e) =>
            {
                var index = (int)Math.Round(scrollView.ContentOffset.X / imageSize.Width);
                if (index < Constants.BSSnapshotList.Count)
                {
                    ViewModel.selectedIndex = index;
                    previousImageView = currentImageView;
                    var trans = currentImageView.Transform;
                    currentImageView = (scrollView.Subviews[index] as UIScrollView).Subviews[0] as UIImageView;
                }
            };

            currentImageView = (scrollView.Subviews[0] as UIScrollView).Subviews[0] as UIImageView;

            */

        }

        private void RightToolbarButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                //currentTransform = currentTransform + (float)(Math.PI / 2);
                //scrollView.Transform = CGAffineTransform.MakeRotation(currentTransform);
                imageView.Image = ScaleAndRotateImage(imageView.Image, UIImageOrientation.Right);
                scrollView.SetZoomScale(1f, true);
            }
            catch (Exception ex)
            {

            }


        }

        private void LeftToolBarButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                //currentTransform = currentTransform - (float)(Math.PI / 2);
                //scrollView.Transform = CGAffineTransform.MakeRotation(currentTransform);
                imageView.Image = ScaleAndRotateImage(imageView.Image, UIImageOrientation.Left);

                scrollView.SetZoomScale(1f, true);
            }
            catch (Exception ex)
            {

            }


        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        private void selectImage(int selectedIndex)
        {
            scrollView.SetContentOffset(new CGPoint(selectedIndex * imageSize.Width, scrollView.Bounds.Y), true);
        }

        static UIImage FromUrl(string uri)
        {
            using (var url = new NSUrl(uri))
            using (var data = NSData.FromUrl(url))
                return UIImage.LoadFromData(data);
        }

        private void BackBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }

        public void loadUIData()
        {
            loadImageViewImage();
            scrollView.ContentSize = imageView.Frame.Size;
            //scrollView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            this.NavigationItem.Title = ViewModel.SnapShot.SnapshotDate.ToString();
            //SCROLL VIEW ZOOM
            scrollView.MaximumZoomScale = 2f;
            scrollView.MinimumZoomScale = 1f;

            scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return imageView; };

        }
        public async Task loadImageViewImage()
        {
            if (ViewModel.SnapShot.ImageDataObj.ImageData != null)
            {
                byte[] imageByteArray = ViewModel.SnapShot.ImageDataObj.ImageData;

                var data = NSData.FromArray(imageByteArray);
                imageView.Image = UIImage.LoadFromData(data);
            }
            else
            {
                using (var url = new NSUrl(ViewModel.SnapShot.ImageDataObj.ImageUrl))
                using (var data = NSData.FromUrl(url))
                    imageView.Image = UIImage.LoadFromData(data);
            }

            //imageView.Image = await this.LoadImage(ViewModel.SnapShot.ImageDataObj.ImageUrl);



        }
        public async Task<UIImage> LoadImage(string imageUrl)
        {
            var httpClient = new HttpClient();

            Task<byte[]> contentsTask = httpClient.GetByteArrayAsync(imageUrl);

            // await! control returns to the caller and the task continues to run on another thread
            var contents = await contentsTask;

            // load from bytes
            return UIImage.LoadFromData(NSData.FromArray(contents));
        }

        UIImage ScaleAndRotateImage(UIImage imageIn, UIImageOrientation orIn)
        {
            int kMaxResolution = 2048;

            CGImage imgRef = imageIn.CGImage;
            float width = imgRef.Width;
            float height = imgRef.Height;
            CGAffineTransform transform = CGAffineTransform.MakeIdentity();
            RectangleF bounds = new RectangleF(0, 0, width, height);

            if (width > kMaxResolution || height > kMaxResolution)
            {
                float ratio = width / height;

                if (ratio > 1)
                {
                    bounds.Width = kMaxResolution;
                    bounds.Height = bounds.Width / ratio;
                }
                else
                {
                    bounds.Height = kMaxResolution;
                    bounds.Width = bounds.Height * ratio;
                }
            }

            float scaleRatio = bounds.Width / width;
            SizeF imageSize = new SizeF(width, height);
            UIImageOrientation orient = orIn;
            float boundHeight;

            switch (orient)
            {
                case UIImageOrientation.Up:                                        //EXIF = 1
                    transform = CGAffineTransform.MakeIdentity();
                    break;

                case UIImageOrientation.UpMirrored:                                //EXIF = 2
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, 0f);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    break;

                case UIImageOrientation.Down:                                      //EXIF = 3
                    transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
                    break;

                case UIImageOrientation.DownMirrored:                              //EXIF = 4
                    transform = CGAffineTransform.MakeTranslation(0f, imageSize.Height);
                    transform = CGAffineTransform.MakeScale(1.0f, -1.0f);
                    break;

                case UIImageOrientation.LeftMirrored:                              //EXIF = 5
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, imageSize.Width);
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Left:                                      //EXIF = 6
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(0.0f, imageSize.Width);
                    transform = CGAffineTransform.Rotate(transform, 3.0f * (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.RightMirrored:                             //EXIF = 7
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                case UIImageOrientation.Right:                                     //EXIF = 8
                    boundHeight = bounds.Height;
                    bounds.Height = bounds.Width;
                    bounds.Width = boundHeight;
                    transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0.0f);
                    transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
                    break;

                default:
                    throw new Exception("Invalid image orientation");
                    break;
            }

            UIGraphics.BeginImageContext(bounds.Size);

            CGContext context = UIGraphics.GetCurrentContext();

            if (orient == UIImageOrientation.Right || orient == UIImageOrientation.Left)
            {
                context.ScaleCTM(-scaleRatio, scaleRatio);
                context.TranslateCTM(-height, 0);
            }
            else
            {
                context.ScaleCTM(scaleRatio, -scaleRatio);
                context.TranslateCTM(0, -height);
            }

            context.ConcatCTM(transform);
            context.DrawImage(new RectangleF(0, 0, width, height), imgRef);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy;
        }

        private void Swipe(UISwipeGestureRecognizer gesture)
        {
            scrollView.ScrollEnabled = false;
            if (gesture.Direction == UISwipeGestureRecognizerDirection.Right)
            {

                ViewModel.SwipeRight();
            }
            if (gesture.Direction == UISwipeGestureRecognizerDirection.Left)
            {

                ViewModel.SwipeLeft();
            }
        }


        private void OnSingleTap(UIGestureRecognizer gesture)
        {
            if (toolBar.Hidden)
            {
                toolBar.Hidden = true;
                this.NavigationController.NavigationBarHidden = false;
            }

            else
            {
                toolBar.Hidden = true;
                this.NavigationController.NavigationBarHidden = true;
            }

        }

        private void OnDoubleTap(UIGestureRecognizer gesture)
        {
            scrollView.ScrollEnabled = true;

            if (scrollView.ZoomScale > 1)
            {
                scrollView.SetZoomScale(1f, true);
            }
            else
            {
                scrollView.SetZoomScale(5, true);
                //scrollView.ScrollEnabled = false;

            }
            CenterImageInScrollView();
        }

        //private void OnDoubleTap(UIGestureRecognizer gesture)
        //{
        //    //scrollView.ScrollEnabled = true;
        //    //scrollView.ZoomScale = 100;
        //    if (scrollView.ZoomScale >= 5)
        //    {
        //        scrollView.SetZoomScale(1f, true);
        //        scrollView.ContentSize = new CGSize(imageSize.Width * Constants.BSSnapshotList.Count, imageSize.Height);
        //        selectImage(ViewModel.selectedIndex);
        //    }
        //    else
        //    {
        //        scrollView.SetZoomScale(5, true);
        //        //scrollView.ScrollEnabled = false;

        //    }
        //    //CenterImageInScrollView();
        //}
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        public void actionOnShareBtn(object sender, EventArgs args)
        {
            if (imageView != null && imageView.Image != null)
            {
                NSObject[] activityItems = { imageView.Image };
                UIActivityViewController activityViewController = new UIActivityViewController(activityItems, null);
                activityViewController.ExcludedActivityTypes = new NSString[] { };
                if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
                {
                    activityViewController.PopoverPresentationController.SourceView = View;
                    activityViewController.PopoverPresentationController.SourceRect = new CoreGraphics.CGRect((View.Bounds.Width / 2), (View.Bounds.Height / 4), 0, 0);
                }
                this.PresentViewController(activityViewController, true, null);
            }
            else
            {
                //show error message

                new DialogService().ShowAlertAsync("Please wait till Image loading completes.", Strings.error, Strings.ok);
            }

        }
        public void actionOnRightRotateBtn(object sender, EventArgs args)
        {
            float currentTransform = ViewModel.SnapShot.ImageTransform;
            ViewModel.SnapShot.ImageTransform = currentTransform + (float)(Math.PI / 2);
            imageView.Transform = CGAffineTransform.MakeRotation(ViewModel.SnapShot.ImageTransform);
        }
        public void actionOnLeftRotateBtn(object sender, EventArgs args)
        {
            float currentTransform = ViewModel.SnapShot.ImageTransform;
            ViewModel.SnapShot.ImageTransform = currentTransform - (float)(Math.PI / 2);
            imageView.Transform = CGAffineTransform.MakeRotation(ViewModel.SnapShot.ImageTransform);
        }
        public Boolean ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            return true;
        }
        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SnapShot"))
            {
                loadUIData();
            }
            else if (e.PropertyName == "selectedIndex")
            {
                //    selectImage(ViewModel.selectedIndex);
            }
        }
        private void CenterImageInScrollView()
        {

            var subView = scrollView.Subviews[0];

            var offsetX = Math.Max((scrollView.Bounds.Size.Width - scrollView.ContentSize.Width) * 0.5, 0.0);
            var offsetY = Math.Max((scrollView.Bounds.Size.Height - scrollView.ContentSize.Height) * 0.5, 0.0);

            subView.Center = new CGPoint(scrollView.ContentSize.Width * 0.5 + offsetX, scrollView.ContentSize.Height * 0.5 + offsetY);

            //var boundsSize = scrollView.Bounds.Size;
            //CGRect frameToCenter = imageView.Frame;

            //if (frameToCenter.Size.Width < boundsSize.Width)
            //    frameToCenter.X = ((int)boundsSize.Width - frameToCenter.Size.Width) / 2;
            //else
            //    frameToCenter.X = 0;

            //if (frameToCenter.Size.Height < boundsSize.Height)
            //    frameToCenter.Y = ((int)boundsSize.Height - frameToCenter.Size.Height) / 2;
            //else
            //    frameToCenter.Y = 0;

            //imageView.Frame = frameToCenter;

        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);
            //gradientView.SetNeedsDisplay();
            RefreshNavigationBar();
        }
    }
}

