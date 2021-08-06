using System;
using BrightSign.Core.Utility;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Utility;
using CoreAnimation;
using CoreGraphics;
using CoreImage;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    public class BaseView<T> : MvxViewController<T> where T : MvxViewModel
    {

        /// <summary>
        /// The height of the keyboard.
        /// </summary>
        public static int KEYBOARD_HEIGHT = 216;

        internal UIBarButtonItem homeButton;
        public BaseView() : base("BaseView", null)
        {

        }

        private void HomeBarButtonItemAction(object sender, EventArgs e)
        {
            (ViewModel as BaseViewModel).ChangeDeviceCommand.Execute();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:actchargers.iOS.BaseView"/> class.
        /// </summary>
        /// <param name="nibName">Nib name.</param>
        /// <param name="bundle">Bundle.</param>
        protected BaseView(string nibName, Foundation.NSBundle bundle, bool addHomeButton = true)
            : base(nibName, bundle)
        {
            if (addHomeButton)
            {
                homeButton = new UIBarButtonItem(UIImage.FromBundle("ic_home_white.png"), UIBarButtonItemStyle.Plain, HomeBarButtonItemAction);
                NavigationItem.LeftBarButtonItem = homeButton;
            }
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NavigationController.NavigationBarHidden = false;
            NavigationItem.HidesBackButton = true;

            SetTitle();

            SetGradientNavigationBar();

        }

        private void SetTitle()
        {
            UILabel titleLabel = new UILabel(new CoreGraphics.CGRect(0, 0, 30, 44));
            titleLabel.BackgroundColor = UIColor.Clear;
            titleLabel.TextColor = UIColor.White;
            titleLabel.Font = UIFont.BoldSystemFontOfSize(16);
            titleLabel.TextAlignment = UITextAlignment.Center;
            titleLabel.Lines = 0;

            var bindingSet = this.CreateBindingSet<BaseView<T>, BaseViewModel>();
            bindingSet.Bind(titleLabel).To((BaseViewModel vm) => vm.ViewTitle);
            bindingSet.Apply();

            NavigationItem.TitleView = titleLabel;
        }

        private void SetGradientNavigationBar()
        {
            CAGradientLayer gradientlayer = new CAGradientLayer();
            gradientlayer.Frame = NavigationController.NavigationBar.Bounds;
            gradientlayer.Colors = new CGColor[] { UIColorUtility.FromHex(ColorConstants.StartColor).CGColor, UIColorUtility.FromHex(ColorConstants.EndColor).CGColor };
            gradientlayer.StartPoint = new CGPoint(0.0, 0.5);
            gradientlayer.EndPoint = new CGPoint(1.0, 0.5);

            UIGraphics.BeginImageContext(gradientlayer.Bounds.Size);
            gradientlayer.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage gradientimage = UIGraphics.GetImageFromCurrentImageContext();

            NavigationController.NavigationBar.SetBackgroundImage(gradientimage, UIBarMetrics.Default);


            CAGradientLayer gradientlayertemp = new CAGradientLayer();
            gradientlayertemp.Frame = new CGRect(NavigationController.NavigationBar.Bounds.X, NavigationController.NavigationBar.Bounds.Y + NavigationController.NavigationBar.Bounds.Height, NavigationController.NavigationBar.Bounds.Width, 1);
            gradientlayertemp.Colors = new CGColor[] { UIColorUtility.FromHex(ColorConstants.StartColor).CGColor, UIColorUtility.FromHex(ColorConstants.EndColor).CGColor };
            gradientlayertemp.StartPoint = new CGPoint(0.0, 0.5);
            gradientlayertemp.EndPoint = new CGPoint(1.0, 0.5);
            UIGraphics.BeginImageContext(gradientlayertemp.Bounds.Size);
            gradientlayertemp.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage gradientimagetemp = UIGraphics.GetImageFromCurrentImageContext();

            NavigationController.NavigationBar.ShadowImage = new UIImage();

            NavigationController.NavigationBar.TintColor = UIColor.White;
        }

        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyBoardShown);
        }

        /// <summary>
        /// On the key board shown.
        /// Calculating the Height of the Keyboard 
        /// </summary>
        /// <param name="notification">Notification.</param>
        public void OnKeyBoardShown(NSNotification notification)
        {
            var keyboardBounds = (NSValue)notification.UserInfo.ObjectForKey(UIKeyboard.BoundsUserInfoKey);
            var keyboardSize = keyboardBounds.RectangleFValue;
            KEYBOARD_HEIGHT = (int)keyboardSize.Height;
        }


        public void RefreshNavigationBar()
        {
            SetGradientNavigationBar();
        }

    }
}
