using System;
using System.Windows.Input;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace BrightSign.Droid.Utility
{
    public class ImageHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; set; }

        public ImageHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.imgRotate1);
            //ItemView.Click += ItemView_Click;
            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
               // ItemView.Click -= ItemView_Click;

            }
        }

        //public ICommand ItemClick
        //{
        //    get; set;
        //}

        //void ItemView_Click(object sender, EventArgs e)
        //{
        //    if (ItemClick == null) return;
        //    if (ItemClick.CanExecute(null))
        //        ItemClick.Execute(null);
        //}
    }
}
