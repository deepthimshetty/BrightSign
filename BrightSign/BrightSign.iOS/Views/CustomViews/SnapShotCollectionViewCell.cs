using System;
using System.Diagnostics;
using BrightSign.Core.Models;
using BrightSign.Core.Utility;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.CustomViews
{
    public partial class SnapShotCollectionViewCell : MvxCollectionViewCell
    {
        public static readonly NSString Key = new NSString("SnapShotCollectionViewCell");
        public static readonly UINib Nib;

        /// <summary>
        /// MvxImageViewLoader for UIImageView
        /// </summary>
        private MvxImageViewLoader loader;

        public int snapshotIndex;
        static SnapShotCollectionViewCell()
        {
            Nib = UINib.FromName("SnapShotCollectionViewCell", NSBundle.MainBundle);
        }

        protected SnapShotCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            loader = new MvxImageViewLoader(() => snapImagVw)
            {
                //DefaultImagePath = "res:" + NSBundle.MainBundle.PathForResource(Constants.ActiveDevice.Image, "png")
            };
            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<SnapShotCollectionViewCell, BSSnapshot>();
                //set.Bind(loader).To(item => item.ImageUrl); //.WithConversion("ImageName", 1);
                set.Bind(snapImagVw).For(o => o.Image).To(item => item.ImageDataObj).WithConversion("ImageData");
                set.Bind(snapImagVw).For(o => o.Transform).To(item => item.ImageTransform).WithConversion("ImageTransform");
                set.Apply();

            });
        }

    }
}
