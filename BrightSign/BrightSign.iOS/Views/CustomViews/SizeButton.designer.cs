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
	partial class SizeButton
	{
		[Outlet]
		UIKit.UIImageView buttonImage { get; set; }

		[Outlet]
		UIKit.UILabel buttonText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (buttonImage != null) {
				buttonImage.Dispose ();
				buttonImage = null;
			}

			if (buttonText != null) {
				buttonText.Dispose ();
				buttonText = null;
			}
		}
	}
}
