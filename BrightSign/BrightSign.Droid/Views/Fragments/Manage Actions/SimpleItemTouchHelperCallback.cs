using System;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;

namespace BrightSign.Droid.Views.Fragments.ManageActions
{
    public class SimpleItemTouchHelperCallback : ItemTouchHelper.Callback
    {

        public ActionItemsAdapter browseItemsAdapter;
        public SimpleItemTouchHelperCallback(ActionItemsAdapter browseItemsAdapter)
        {
            this.browseItemsAdapter = browseItemsAdapter;
        }

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            int SwipeFlags = ItemTouchHelper.ActionStateIdle;
            return MakeMovementFlags(dragFlags, SwipeFlags);

            //throw new NotImplementedException();
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
             browseItemsAdapter.onItemMove(viewHolder.AdapterPosition, target.AdapterPosition);
            return true;
            //throw new NotImplementedException();
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            //throw new NotImplementedException();
        }


        public override bool IsItemViewSwipeEnabled => false;
        public override bool IsLongPressDragEnabled => false;

    }
}
