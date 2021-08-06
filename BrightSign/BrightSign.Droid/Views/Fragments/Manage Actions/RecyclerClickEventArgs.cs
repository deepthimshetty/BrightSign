using System;
using Android.Views;

namespace BrightSign.Droid.Views.Fragments.ManageActions
{
    public class RecyclerClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public bool isDrag { get; set; }
    }
}
