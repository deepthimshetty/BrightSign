using System;
using System.Linq;
using BrightSign.Core.ViewModels;
using BrightSign.iOS.Views.BSUnits;
using MvvmCross.iOS.Views;
using UIKit;

namespace BrightSign.iOS
{
    public partial class BSSelectUnitsViewController : MvxViewController<BSUnitsViewModel>
    {
        public BSSelectUnitsViewController() : base("BSSelectUnitsViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            try {
                base.ViewDidLoad();

                Title = ViewModel.ViewTitle;

                //UILabel lblTitle = new UILabel();
                //lblTitle.Text = ViewModel.ViewTitle;
                //lblTitle.BackgroundColor = UIColor.Clear;
                //lblTitle.TextColor = UIColor.Black;
                //lblTitle.Font = UIFont.BoldSystemFontOfSize(16);
                //NavigationItem.TitleView = lblTitle;

                UIBarButtonItem cancelBtn = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelBarButtonItemAction);
                UIBarButtonItem refreshBtn = new UIBarButtonItem(UIBarButtonSystemItem.Refresh, RefreshBarButtonItemAction);
                NavigationItem.LeftBarButtonItems = new UIBarButtonItem[2] {cancelBtn, refreshBtn };

                UIBarButtonItem saveBtn = new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveBarButtonItemAction);
                NavigationItem.RightBarButtonItem = saveBtn;

                unitsTableView.Source = new BSSelectUnitsTableViewSource(unitsTableView, ViewModel);
            } catch (Exception ex) {
                
            }
            // Perform any additional setup after loading the view, typically from a nib.
        }

        private void SaveBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute();
        }

        private void CancelBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }

        private void RefreshBarButtonItemAction(object sender, EventArgs e)
        {
            
        }

        public void executecmnd() {
            
        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


    }
}

