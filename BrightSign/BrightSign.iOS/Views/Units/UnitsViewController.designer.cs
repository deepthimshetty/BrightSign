// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace BrightSign.iOS.Views.Units
{
    [Register ("UnitsViewController")]
    partial class UnitsViewController
    {
        [Outlet]
        BrightSign.iOS.Views.Home.CustomViews.TabView activeTab { get; set; }


        [Outlet]
        BrightSign.iOS.Views.CustomViews.RoundedButton addDeviceButton { get; set; }


        [Outlet]
        UIKit.UITableView devicesTableView { get; set; }


        [Outlet]
        BrightSign.iOS.Views.CustomViews.GradientView gradientView { get; set; }


        [Outlet]
        BrightSign.iOS.Views.Home.CustomViews.TabView offlineTab { get; set; }

        void ReleaseDesignerOutlets ()
        {
        }
    }
}