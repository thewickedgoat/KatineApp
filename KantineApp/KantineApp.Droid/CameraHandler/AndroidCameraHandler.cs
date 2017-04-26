using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KantineApp.Interface;
using Xamarin.Forms;
using KantineApp.Droid.CameraHandler;

[assembly: Dependency(typeof(AndroidCameraHandler))]

namespace KantineApp.Droid.CameraHandler 
{
    class AndroidCameraHandler : ICameraHandler
    {
        private static event PhotoTaken PhotoTakenEvent;

        public void AddPhotoTakenEventHandler(PhotoTaken pt)
        {
            PhotoTakenEvent += pt;

        }

        public void TakePhoto()
        {
            Activity activity = (Activity)Forms.Context;
            Intent intent = new Intent();
            intent.SetClass(activity, typeof(CameraMediator));
            activity.StartActivity(intent);
        }

        public static void TakenPicture(string fileName)
        {
            PhotoTakenEvent.Invoke(fileName);
        }
    }
}