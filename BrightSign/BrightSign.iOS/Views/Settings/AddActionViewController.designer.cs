// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace BrightSign.iOS.Views.Settings
{
	[Register ("AddActionViewController")]
	partial class AddActionViewController
	{
		[Outlet]
		UIKit.UITextField dataTxtFld { get; set; }

		[Outlet]
		UIKit.UITextField labelTxtFld { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (labelTxtFld != null) {
				labelTxtFld.Dispose ();
				labelTxtFld = null;
			}

			if (dataTxtFld != null) {
				dataTxtFld.Dispose ();
				dataTxtFld = null;
			}
		}
	}
}
