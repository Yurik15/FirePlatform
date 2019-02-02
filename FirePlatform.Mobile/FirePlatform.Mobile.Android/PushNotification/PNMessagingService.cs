using System;
using System.Collections.Generic;
using Android.App;
using Firebase.Messaging;
using FirePlatform.Mobile.Common.PushNotification;

namespace FirePlatform.Mobile.Droid.PushNotification
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class PNMessagingService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            var parameters = new Dictionary<string, object>();
            var notification = message.GetNotification();

            foreach (var d in message.Data)
            {
                if (!parameters.ContainsKey(d.Key))
                    parameters.Add(d.Key, d.Value);
            }
            PushNotificationManager.RegisterData(parameters);
            if (CrossPushNotification.AppIsClosed)
                CrossPushNotification.Current.NotificationHandler.OnReceived(parameters);
        }

        public override void OnMessageSent(string msgId)
        {
            base.OnMessageSent(msgId);
        }
    }

}
