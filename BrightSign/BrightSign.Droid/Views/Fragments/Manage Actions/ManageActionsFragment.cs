
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BrightSign.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using Android.Support.Design.Widget;
using BrightSign.Droid.Utility.Interface;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using static Android.Views.View;

namespace BrightSign.Droid.Views.Fragments.ManageActions
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class ManageActionsFragment : MvxFragment<ManageActionsViewModel>, OnStartDragListener
    {
        View view;
        View toolbar;
        FloatingActionButton btnFloating;
        Button btnCancel;
        Button defaultBtn, userDefinedBtn;
        View defaultView, userDefinedView;
        ActionItemsAdapter adapter;
        ItemTouchHelper touchHelper;
        RecyclerView recyclerView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            base.OnCreateView(inflater, container, savedInstanceState);
            view = this.BindingInflate(Resource.Layout.manage_actions, null);
            ((MainActivity)this.Activity).HideAllIcons();

            toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
            var txtTitle = toolbar.FindViewById<TextView>(Resource.Id.TitleText);
            txtTitle.Text = ViewModel.ViewTitle;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            recyclerView =
               view.FindViewById<Android.Support.V7.Widget.RecyclerView>(Resource.Id.actionsRecyclerView);


            adapter = new ActionItemsAdapter(Activity, ViewModel, this,btnCancel);

            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter);

            ItemTouchHelper.Callback callback = new SimpleItemTouchHelperCallback(adapter);

            touchHelper = new ItemTouchHelper(callback);
            touchHelper.AttachToRecyclerView(recyclerView);


            btnFloating = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            var topView = view.FindViewById(Resource.Id.actions_top_view);
            var actionsListView = view.FindViewById<MvxListView>(Resource.Id.gvActions);

            var cancelButton = view.FindViewById<Button>(Resource.Id.btnCancel);
            var updateButton = view.FindViewById<Button>(Resource.Id.btnUpdate);

            cancelButton.Click += CancelButton_Click;
            updateButton.Click += UpdateButton_Click;

            btnCancel = toolbar.FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Visibility = ViewStates.Visible;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            var btnEdit = toolbar.FindViewById<Button>(Resource.Id.btnSave);
            btnEdit.Visibility = ViewStates.Gone;

            TextView deviceName = topView.FindViewById<TextView>(Resource.Id.top_device_name);
            deviceName.Text = ViewModel.CurrentDevice.Name;
            TextView deviceIP = topView.FindViewById<TextView>(Resource.Id.top_device_ip);
            deviceIP.Text = "IP Address:" + ViewModel.CurrentDevice.IpAddress;
            ImageView deviceImg = topView.FindViewById<ImageView>(Resource.Id.top_deviceImg);
            //ImageView arrowImg = topView.FindViewById<ImageView>(Resource.Id.arrow);
            //arrowImg.Visibility = ViewStates.Gone;

            defaultBtn = view.FindViewById<Button>(Resource.Id.defaultBtn);
            defaultBtn.Click += BtnDefault_Click;
            userDefinedBtn = view.FindViewById<Button>(Resource.Id.userDefinedBtn);
            userDefinedBtn.Click += BtnUserDefined_Click;

            defaultView = view.FindViewById(Resource.Id.defaultView);
            userDefinedView = view.FindViewById(Resource.Id.userDefinedView);

            SelectDefaultTab();

            //Registering Context Menu for ListView
            //RegisterForContextMenu(actionsListView);

            //RegisterForContextMenu(recyclerView);

            return view;
        }

        private void SelectDefaultTab()
        {
            if (ViewModel.SelectedTabIndex == 0)
            {
                defaultView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
                userDefinedView.SetBackgroundColor(Android.Graphics.Color.White);
            }
            else
            {
                userDefinedView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
                defaultView.SetBackgroundColor(Android.Graphics.Color.White);
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsEditViewVisible"))
            {
                btnCancel.Enabled = ViewModel.IsEditViewVisible ? false : true;

                if (ViewModel.IsAddButtonVisible)
                    btnFloating.Visibility = ViewModel.IsEditViewVisible ? ViewStates.Gone : ViewStates.Visible;
            }
            else if (e.PropertyName.Equals("IsRefresh"))
            {
                if (ViewModel.IsRefresh)
                {
                    adapter.refresh();
                    //recyclerView.Invalidate();
                    ViewModel.IsRefresh = false;
                }
            } else if(e.PropertyName.Equals("SelectedTabIndex")){
                adapter.refresh();
            }
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            ((MainActivity)this.Activity).HideKeybord();
            // ViewModel.CancelEditCommand.Execute();
        }

        void UpdateButton_Click(object sender, EventArgs e)
        {
            ((MainActivity)this.Activity).HideKeybord();
            //ViewModel.UpdateCommand.Execute();
        }

        void BtnCancel_Click(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            btnCancel.Click -= BtnCancel_Click;
        }
        void BtnDefault_Click(object sender, EventArgs e)
        {
            defaultView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
            userDefinedView.SetBackgroundColor(Android.Graphics.Color.White);
            ViewModel.SelectedTabIndex = 0;
        }
        void BtnUserDefined_Click(object sender, EventArgs e)
        {
            userDefinedView.SetBackgroundColor(Android.Graphics.Color.DarkOrange);
            defaultView.SetBackgroundColor(Android.Graphics.Color.White);
            ViewModel.SelectedTabIndex = 1;
        }


        public override void OnStart()
        {
            base.OnStart();

            adapter.ItemClick += Adapter_ItemClick;

            //if (ViewModel.Items.Count == 0)
            //ViewModel.LoadItemsCommand.Execute(null);
        }

        public override void OnStop()
        {
            base.OnStop();
            adapter.ItemClick -= Adapter_ItemClick;
        }

        void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
        {
            var item = ViewModel.ActionsItemSource[e.Position];
        }


        public void viewDragged(RecyclerView.ViewHolder viewHolder)
        {
            touchHelper.StartDrag(viewHolder);
        }


        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            //Need to check with ListView ID
            if (v.Id == Resource.Id.gvActions)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                //menu.SetHeaderTitle(_countries[info.Position]);
                var menuItems = Resources.GetStringArray(Resource.Array.action_menu);


                for (var i = 0; i < menuItems.Length; i++)
                {
                    if (ViewModel.SelectedTabIndex == 0 && menuItems[i].ToLower().Equals("delete"))
                    {
                        continue;
                    }
                    menu.Add(Menu.None, i, i, menuItems[i]);
                }

            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            try
            {
                var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
                var menuItemIndex = item.ItemId;
                var menuItems = Resources.GetStringArray(Resource.Array.action_menu);
                var menuItemName = menuItems[menuItemIndex];
                if (menuItemName.ToLower().Equals("edit"))
                {
                    //var editLayout = view.FindViewById<RelativeLayout>(Resource.Id.editlayout);
                    //editLayout.Visibility = ViewStates.Visible;
                    ViewModel.EditCommand.Execute(info.Position);
                    btnCancel.Enabled = false;
                }
                else
                {
                    ViewModel.RemoveCommand.Execute(info.Position);
                }
                //var listItemName = _countries[info.Position];
                //ViewModel.DeleteUnitCommand.Execute(info.Position);
            }
            catch (Exception ex)
            {

            }
            //Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName,1), ToastLength.Short).Show();
            return true;
        }

        public void onStartDrag(RecyclerView.ViewHolder viewHolder)
        {
            touchHelper.StartDrag(viewHolder);
        }
    }


    public class ActionItemsAdapter : BaseRecycleViewAdapter, ItemTouchHelperAdapter, IMenuItemOnMenuItemClickListener, View.IOnLongClickListener
    {
        ManageActionsViewModel viewModel;
        Activity activity;
        OnStartDragListener dragListener;
        int position;

        public ActionItemsAdapter(Activity activity, ManageActionsViewModel viewModel, OnStartDragListener dragListener,Button cancelBtn)
        {
            this.viewModel = viewModel;
            this.activity = activity;
            this.dragListener = dragListener;

        }

        public void ItemMoved(int from, int to)
        {
            NotifyItemMoved(from, to);
        }

        public void refresh(){
            NotifyDataSetChanged();
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.action_item;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new MyViewHolder(itemView, OnClick, OnLongClick, this);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = viewModel.ActionsItemSource[position];

            // Replace the contents of the view with that element
            var myHolder = holder as MyViewHolder;
            myHolder.TextView.Text = item.Label;
            //myHolder.DetailTextView.Text = item.Description;

            myHolder.DragButton.Touch += (s, e) =>
            {
                if (e.Event.Action == MotionEventActions.Down)
                {
                    dragListener.onStartDrag(holder);
                    // do stuff

                }
            };

            myHolder.ItemView.SetOnLongClickListener(this);

            //myHolder.ItemView.SetOnLongClickListener(new View.IOnLongClickListener(){

            //    public override bool OnLongClick(View v)
            //    {
            //        return false;
            //     }
            //});




        }

        public bool onItemMove(int fromPosition, int toPosition)
        {
            viewModel.swapItem(fromPosition, toPosition);
            NotifyItemMoved(fromPosition, toPosition);
            return true;
            //throw new NotImplementedException();
        }

        public void onItemDismiss(int position)
        {
            throw new NotImplementedException();
        }

        public bool OnMenuItemClick(IMenuItem item)
        {
            try
            {
                if (item.ItemId == 1)
                {
                    viewModel.EditCommand.Execute(item.Order);

                }
                else
                {
                    viewModel.RemoveCommand.Execute(item.Order);


                }

            }
            catch (Exception ex)
            {

            }
            //Toast.MakeText(this, string.Format("Selected {0} for item {1}", menuItemName,1), ToastLength.Short).Show();
            return true;
        }

        public bool OnLongClick(View v)
        {
            return false;
        }

        public override int ItemCount => viewModel.ActionsItemSource.Count;
    }

    public class MyViewHolder : RecyclerView.ViewHolder, View.IOnCreateContextMenuListener

    {
        public TextView TextView { get; set; }
        //public TextView DetailTextView { get; set; }
        public ImageButton DragButton { get; set; }
        IMenuItemOnMenuItemClickListener menuItemClickListener;

        private void swap()
        {
        }

        public void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            IMenuItem Edit = menu.Add(Menu.None, 1, AdapterPosition, "Edit");
            IMenuItem Delete = menu.Add(Menu.None, 2, AdapterPosition, "Delete");
            Edit.SetOnMenuItemClickListener(menuItemClickListener);
            Delete.SetOnMenuItemClickListener(menuItemClickListener);
        }

        public MyViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
                            Action<RecyclerClickEventArgs> longClickListener, IMenuItemOnMenuItemClickListener menuItemClickListener) : base(itemView)
        {
            this.menuItemClickListener = menuItemClickListener;
            TextView = itemView.FindViewById<TextView>(Resource.Id.actionText);
            //DetailTextView = itemView.FindViewById<TextView>(Android.Resource.Id.Text2);
            DragButton = itemView.FindViewById<ImageButton>(Resource.Id.dragButton);

            itemView.SetOnCreateContextMenuListener(this);

        }
    }
}
