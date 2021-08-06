using System;
using System.Linq;
using BrightSign.Core.ViewModels;
using BrightSign.Core.ViewModels.Settings;
using MvvmCross.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.BSUnits
{
    public partial class BSManageUnitsViewController : MvxViewController<ManageBSUnitsViewModel>
    {
        public BSManageUnitsViewController() : base("BSManageUnitsViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = ViewModel.ViewTitle;

            //UILabel lblTitle = new UILabel();
            //lblTitle.Text = ViewModel.ViewTitle;
            //lblTitle.BackgroundColor = UIColor.Clear;
            //lblTitle.TextColor = UIColor.Black;
            //lblTitle.Font = UIFont.BoldSystemFontOfSize(16);
            //NavigationItem.TitleView = lblTitle;

            UIBarButtonItem doneBtn = new UIBarButtonItem(UIBarButtonSystemItem.Done, DoneBarButtonItemAction);
            NavigationItem.LeftBarButtonItem = doneBtn;

            unitsTableView.Source = new BSManageUnitsTableViewSource(unitsTableView, ViewModel);
            //unitsTableView.SetEditing(true, false);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        private void DoneBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute();
        }
    }
}

