using System;
using Android.Support.V7.Widget;
namespace BrightSign.Droid.Views.Fragments.ManageActions
{
    public interface OnStartDragListener
    {
        void onStartDrag(RecyclerView.ViewHolder viewHolder);
    }
}
