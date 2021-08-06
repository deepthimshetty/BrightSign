// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BrightSign.iOS.Views.CustomViews
{
	partial class SearchDeviceItemTemplate
	{
		[Outlet]
		UIKit.UILabel ipAddress { get; set; }

		[Outlet]
		BrightSign.iOS.Views.CustomViews.ShadowView shadowView { get; set; }

		[Outlet]
		UIKit.UIImageView unitImage { get; set; }

		[Outlet]
		UIKit.UILabel unitName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (unitName != null) {
				unitName.Dispose ();
				unitName = null;
			}

			if (ipAddress != null) {
				ipAddress.Dispose ();
				ipAddress = null;
			}

			if (unitImage != null) {
				unitImage.Dispose ();
				unitImage = null;
			}

			if (shadowView != null) {
				shadowView.Dispose ();
				shadowView = null;
			}
		}
	}
}
