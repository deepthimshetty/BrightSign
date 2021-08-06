
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using BrightSign.Core.Utility;
using BrightSign.Core.Utility.Interface;
using BrightSign.Core.ViewModels;
using BrightSign.Droid.Utility.Interface;
using BrightSign.Localization;
using ImageViews.Photo;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using Square.Picasso;

namespace BrightSign.Droid.Views.Fragments.Snapshot
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    public class SnapshotDetailFragment : MvxFragment<ShareViewModel>
    {
        View toolbar;
        Button btnBack;
        ImageButton btnShare;
        View layoutView;
        ImageButton btnLeft, btnRight;
        PhotoView imgSnapshot;
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
            ((MainActivity)this.Activity).HideAllIcons();

            layoutView = this.BindingInflate(Resource.Layout.SnapshotDetailView, null);
            imgSnapshot = layoutView.FindViewById<PhotoView>(Resource.Id.imgSnapshot);

            //var photo = new PhotoView(this);
            //photo.SetImageBitmap(newBitmap);
            //var bitmap = GetImageBitmapFromUrl(ViewModel.SnapShot.ImageUrl);
            //imgSnapshot.SetImageBitmap(BitmapFactory.DecodeFile(ViewModel.SnapShot.ImageUrl));
            //imgSnapshot.SetImageURI(Android.Net.Uri.Parse(ViewModel.SnapShot.ImageUrl));
            //imgSnapshot.Invalidate();

            Picasso.With(Android.App.Application.Context).Load("file://" + ViewModel.SnapShot.ImageDataObj.ImagePath).Config(Bitmap.Config.Rgb565).Fit().Into(imgSnapshot);

            //if (ViewModel.SnapShot.IsCredentialsRequired)
            //{
            //    Picasso.With(Android.App.Application.Context).Load("file://" + ViewModel.SnapShot.ImageDataObj.ImagePath).Config(Bitmap.Config.Rgb565).Fit().Into(imgSnapshot);
            //}
            //else
            //{
            //    Picasso.With(imgSnapshot.Context).Load(ViewModel.SnapShot.ImageDataObj.ImageUrl).Into(imgSnapshot);
            //}


            ShowToolbarActions();
            return layoutView;
        }

        async void BtnShare_Click(object sender, EventArgs e)
        {

            //Drawable d = imgSnapshot.Background;
            //BitmapDrawable bitDw = ((BitmapDrawable)d);
            //Bitmap bitmap1 = bitDw.Bitmap;


            var fetchedDrawable = imgSnapshot.Drawable;
            BitmapDrawable bitmapDrawable = (BitmapDrawable)fetchedDrawable;
            if (bitmapDrawable != null)
            {
                var bitmap = bitmapDrawable.Bitmap;
                if (bitmap != null)
                {
                    //Android.Net.Uri mFileURI = "file://" +  ViewModel.SnapShot.ImageDataObj.ImagePath;

                    Android.Net.Uri mFileURI = await savebitmap(bitmap);

                    //Bitmap bitmap = Bitmap.CreateBitmap(imgSnapshot.Width, imgSnapshot.Height,
                    //    Bitmap.Config.Argb8888);
                    //Canvas canvas = new Canvas(bitmap);
                    //imgSnapshot.Draw(canvas);

                    Intent shareIntent = new Intent();
                    shareIntent.SetAction(Intent.ActionSend);
                    shareIntent.PutExtra(Intent.ExtraStream, mFileURI);
                    shareIntent.SetType("image/png");
                    ((MainActivity)this.Activity).StartActivity(Intent.CreateChooser(shareIntent, "Share"));
                }
                else
                {
                    await new DialogService().ShowAlertAsync("Please wait till Image loading completes.", Strings.error, Strings.ok);
                }

            }
            else
            {
                await new DialogService().ShowAlertAsync("Please wait till Image loading completes.", Strings.error, Strings.ok);
            }

        }

        async Task<bool> CheckPermission()
        {
            bool hasPermission = false;
            if (await Plugin.Permissions.CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Storage) != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                var permissionResponse = await Plugin.Permissions.CrossPermissions.Current.RequestPermissionsAsync(new Plugin.Permissions.Abstractions.Permission[] { Plugin.Permissions.Abstractions.Permission.Storage });
                if (permissionResponse.ContainsKey(Plugin.Permissions.Abstractions.Permission.Storage))
                {
                    if (permissionResponse[Plugin.Permissions.Abstractions.Permission.Storage] == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                    {
                        hasPermission = true;
                    }
                }
                else
                {

                    await new DialogService().ShowAlertAsync("Please provide storage permission to share the snapshot", Strings.error, Strings.ok);
                }

            }
            else
            {
                hasPermission = true;
            }
            return hasPermission;
        }

        private async Task<Android.Net.Uri> savebitmap(Bitmap bitmap)
        {

            //String filePath = Mvx.Resolve<IFileManager>().GetFilePath();// Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //string filePath = System.IO.Path.Combine(path, "BrightSign.png");

            //var stream = new FileStream(filePath, FileMode.Create);
            //bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            //stream.Close();

            if (!await CheckPermission())
            {
                return null;
            }

            //if (!String.IsNullOrEmpty(ViewModel.SnapShot.ImageDataObj.ImagePath))
            //{
            //    string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

            //    string filePath = System.IO.Path.Combine(path, "BrightSign.png");

            //    var stream = new FileStream(filePath, FileMode.Create);

            //    stream.Write(ViewModel.SnapShot.ImageDataObj.ImageData,0,ViewModel.SnapShot.ImageDataObj.ImageData.Length);
            //    //bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            //    stream.Close();

            //    return Android.Net.Uri.Parse("file://" + filePath);

            //    //return Android.Net.Uri.Parse("file://" + ViewModel.SnapShot.ImageDataObj.ImagePath);
            //}
            //else
            //{
                string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                string filePath = System.IO.Path.Combine(path, "BrightSign.png");

                var stream = new FileStream(filePath, FileMode.Create);
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Close();

                return Android.Net.Uri.Parse("file://" + filePath);

            //}

        }

        static float rotation = 0;
        void BtnLeft_Click(object sender, EventArgs e)
        {
            //rotation = rotation - 1;
            //imgSnapshot.Rotation = rotation * 90;
            imgSnapshot.SetRotationBy(-90);
            imgSnapshot.LayoutParameters = new LinearLayout.LayoutParams(width: LinearLayout.LayoutParams.MatchParent, height: LinearLayout.LayoutParams.MatchParent);


        }

        void BtnRight_Click(object sender, EventArgs e)
        {
            //if (rotation > 0)
            //{
            //rotation = rotation + 1;
            //imgSnapshot.Rotation = rotation * 90;
            //}
            imgSnapshot.SetRotationBy(90);
            imgSnapshot.LayoutParameters = new LinearLayout.LayoutParams(width: LinearLayout.LayoutParams.MatchParent, height: LinearLayout.LayoutParams.MatchParent);

        }


        public override void OnDestroyView()
        {
            base.OnDestroyView();
            UnRegisterEvents();
            // ShowToolbarActions(false);
        }

        public void UnRegisterEvents()
        {
            try
            {
                btnBack.Click -= BtnBack_Click;
                btnShare.Click -= BtnShare_Click;
                FreeImageMemory();
                btnLeft.Click -= BtnLeft_Click;
                btnRight.Click -= BtnRight_Click;
            }
            catch (Exception ex)
            {

            }
        }

        void BtnBack_Click(object sender, EventArgs e)
        {
            ViewModel.CancelCommand.Execute();
        }

        public void ShowToolbarActions()
        {
            toolbar = ((MainActivity)this.Activity).FindViewById(Resource.Id.toolbar);
            btnShare = toolbar.FindViewById<ImageButton>(Resource.Id.btnShare);

            var txtTitle = ((MainActivity)this.Activity).FindViewById<TextView>(Resource.Id.TitleText);

            txtTitle.Text = ViewModel.ViewTitle;

            btnBack = ((MainActivity)this.Activity).FindViewById<Button>(Resource.Id.btnBack);
            btnBack.Visibility = ViewStates.Visible;
            btnShare.Visibility = ViewStates.Visible;
            //Commented Rotate functionality
            var imgrotateLayout = toolbar.FindViewById<LinearLayout>(Resource.Id.rotateLayout);
            imgrotateLayout.Visibility = ViewStates.Visible;
            btnLeft = toolbar.FindViewById<ImageButton>(Resource.Id.btnRotateLeft);
            btnRight = toolbar.FindViewById<ImageButton>(Resource.Id.btnRotateRight);

            ////Registering Toolbar button Events 
            btnBack.Click += BtnBack_Click;
            btnLeft.Click += BtnLeft_Click;
            btnRight.Click += BtnRight_Click;
            btnShare.Click += BtnShare_Click;
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap image = null;


            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    //imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

                    BitmapRegionDecoder decoder = BitmapRegionDecoder.NewInstance(imageBytes, 0, imageBytes.Length, false);
                    image = decoder.DecodeRegion(new Rect(10, 10, 50, 50), null);
                }
            }

            return image;
            //else
            // return resizeAndRotate(imageBitmap,imageBitmap.Width,imageBitmap.Height);
        }

        private void FreeImageMemory()
        {
            imgSnapshot = layoutView.FindViewById<PhotoView>(Resource.Id.imgSnapshot);
            imgSnapshot.SetImageBitmap(null);
            // imgSnapshot..Recycle();
            imgSnapshot.Dispose();
            imgSnapshot = null;
        }

        //private void initShareIntent(String type, String _text)
        //{
        //    File filePath = getFileStreamPath("shareimage.jpg");  //optional //internal storage
        //    Intent shareIntent = new Intent();
        //    shareIntent.setAction(Intent.ACTION_SEND);
        //    shareIntent.putExtra(Intent.EXTRA_TEXT, _text);
        //    shareIntent.putExtra(Intent.EXTRA_STREAM, Uri.fromFile(new File(filePath)));  //optional//use this when you want to send an image
        //    shareIntent.setType("image/jpeg");
        //    shareIntent.addFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        //    startActivity(Intent.createChooser(shareIntent, "send"));
        //}

    }
}
