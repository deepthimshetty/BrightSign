using System;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using UIKit;

namespace BrightSign.iOS.Views.Settings
{
    public partial class AddActionViewController : MvxViewController<AddActionViewModel>
    {
        public AddActionViewController() : base("AddActionViewController", null)
        {

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            UIBarButtonItem cancelBtn = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, CancelBarButtonItemAction);
            NavigationItem.LeftBarButtonItem = cancelBtn;

            UIBarButtonItem saveBtn = new UIBarButtonItem(UIBarButtonSystemItem.Save, SaveBarButtonItemAction);
            NavigationItem.RightBarButtonItem = saveBtn;

            this.CreateBinding(labelTxtFld).To((AddActionViewModel vm) => vm.Label).Apply();
            this.CreateBinding(dataTxtFld).To((AddActionViewModel vm) => vm.Data).Apply();

            labelTxtFld.ShouldReturn += (textField) =>
            {
                dataTxtFld.BecomeFirstResponder();
                return true;
            };
            dataTxtFld.ShouldReturn += (textField) =>
            {
                dataTxtFld.ResignFirstResponder();
                return true;
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void SaveBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.SaveCommand.Execute();
        }

        private void CancelBarButtonItemAction(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }
    }
}

