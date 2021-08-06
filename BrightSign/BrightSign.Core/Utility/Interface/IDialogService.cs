using System;
using System.Threading.Tasks;

namespace BrightSign.Core.Utility.Interface
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonText);

        void ShowLoading(string loadingText = null);

        void HideLoading();

        void ShowAlertWithTwoButtons(string message, string title = null, string okButtonText = "OK", string cancelButtonText = "Cancel", Action okClicked = null, Action cancelClikced = null);

        void ShowActionSheetAlert(string title, string cancelstr, string item1, string item2, Action item1Clicked = null, Action item2Clikced = null);
    }
}
