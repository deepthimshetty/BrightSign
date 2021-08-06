using System;
using BrightSign.Core.ViewModels;
using BrightSign.Core.ViewModels.SearchUnits;
using MvvmCross.Core.ViewModels;

namespace BrightSign.Core
{
    public class AppStart : MvxNavigatingObject, IMvxAppStart
    {
        public AppStart()
        {
        }

        public void Start(object hint = null)
        {
            ShowViewModel<SearchUnitsViewModel>();

            //ShowViewModel<MainViewModel>();
            //ShowViewModel<SnapshotsViewModel>();

        }
    }
}
