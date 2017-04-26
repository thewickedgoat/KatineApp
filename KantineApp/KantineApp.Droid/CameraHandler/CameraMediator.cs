

namespace KantineApp.Droid.CameraHandler
{
    [Activity(Label = "CameraMediator")]
    public class CameraMediator : Activity
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.C);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();


                Intent intent = new Intent(MediaStore.ActionImageCapture);
                _file = new File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));
                StartActivityForResult(intent, 0);
                Toast.MakeText(this, "Build-in camera app started!", ToastLength.Long).Show();
            }
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new File(
                Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Toast.MakeText(this, "Build-in camera returned a picture: " + _file.AbsolutePath, ToastLength.Long).Show();
            AndroidCameraHandler.TakenPicture(_file.AbsolutePath);
            Finish();
        }



    }


}
