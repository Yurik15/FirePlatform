using System;
using Android.App;
using Android.Content;
using Firebase.Iid;

namespace FirePlatform.Mobile.Droid.PushNotification
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class PNIIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;

            var editor = Android.App.Application.Context.GetSharedPreferences(PushNotificationManager.KeyGroupName, FileCreationMode.Private).Edit();
            editor.PutString(PushNotificationManager.TokenKey, refreshedToken);
            editor.Commit();

            PushNotificationManager.RegisterToken(refreshedToken);
        }
    }
}
