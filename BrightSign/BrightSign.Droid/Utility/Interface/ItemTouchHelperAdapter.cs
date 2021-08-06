using System;
namespace BrightSign.Droid.Utility.Interface
{
    public interface ItemTouchHelperAdapter
    {
         bool onItemMove(int fromPosition, int toPosition);

        void onItemDismiss(int position);
    }
}
