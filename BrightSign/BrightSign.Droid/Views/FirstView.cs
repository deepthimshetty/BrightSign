using Android.App;
using Android.OS;

namespace BrightSign.Droid.Views
{
    [Activity(Label = "View for FirstViewModel")]
    public class FirstView : BaseView
    {
        protected override int LayoutResource => Resource.Layout.MainView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
        }
    }
}
