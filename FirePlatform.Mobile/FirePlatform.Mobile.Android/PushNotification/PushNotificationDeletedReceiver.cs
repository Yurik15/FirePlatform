﻿using System;
using System.Collections.Generic;
using Android.Content;

namespace FirePlatform.Mobile.Droid.PushNotification
{
    [BroadcastReceiver]
    public class PushNotificationDeletedReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            var extras = intent.Extras;

            if (extras != null && !extras.IsEmpty)
            {
                foreach (var key in extras.KeySet())
                {
                    parameters.Add(key, $"{extras.Get(key)}");
                    System.Diagnostics.Debug.WriteLine(key, $"{extras.Get(key)}");
                }
            }

            PushNotificationManager.RegisterDelete(parameters);
        }
    }
}
