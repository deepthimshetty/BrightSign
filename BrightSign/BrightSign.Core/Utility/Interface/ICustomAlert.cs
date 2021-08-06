using System;
namespace BrightSign.Core.Utility.Interface
{
    public interface ICustomAlert
    {
        void ShowCustomAlert(bool isSuccess, string title, string message);

    }
}
