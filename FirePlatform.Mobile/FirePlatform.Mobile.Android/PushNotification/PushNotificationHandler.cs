using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using FirePlatform.Mobile.Common.PushNotification.Abstractions;

namespace FirePlatform.Mobile.Droid.PushNotification
{
    public class PushNotificationHandler : IPushNotificationHandler
    {
        #region consts
        public const string DomainTag = nameof(PushNotificationHandler);
        public const string ChannelIdKey = "android_channel_id";
        public const string IdKey = "id";
        public const string ActionNotificationIdKey = "action_notification_id";
        public const string ActionNotificationTagKey = "action_notification_tag";
        public const string ActionIdentifierKey = "action_identifier";
        #endregion consts

        public void OnError(string error)
        {
            System.Diagnostics.Debug.WriteLine($"{DomainTag} - OnError");
        }

        public void OnOpened(NotificationResponse response)
        {
            System.Diagnostics.Debug.WriteLine($"{DomainTag} - OnOpened");
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
            System.Diagnostics.Debug.WriteLine($"{DomainTag} - OnReceived");

            int notifyId = 0;
            var title = string.Empty;
            var message = string.Empty;
            var badgeNumber = 0;

            /*if (parameters.ContainsKey(NotificationFields.TitleField))
            {
                title = parameters[NotificationFields.TitleField].ToString();
            }
            if (parameters.ContainsKey(NotificationFields.BodyField))
            {
                message = parameters[NotificationFields.BodyField].ToString();
            }
            if (parameters.ContainsKey(NotificationFields.BadgerField))
            {
                if (int.TryParse(parameters[NotificationFields.BadgerField].ToString(), out int result))
                {
                    badgeNumber = result;
                }

            }*/

            if (parameters.TryGetValue(IdKey, out object id))
            {
                try
                {
                    notifyId = Convert.ToInt32(id);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to convert {id} to an integer {ex}");
                }
            }

            Android.Content.Context context = Application.Context;

            var chanId = PushNotificationManager.DefaultNotificationChannelId;
            if (parameters.TryGetValue(ChannelIdKey, out object channelId) && channelId != null)
            {
                chanId = $"{channelId}";
            }
            Intent resultIntent = typeof(Activity).IsAssignableFrom(PushNotificationManager.NotificationActivityType) ?
                new Intent(Application.Context, PushNotificationManager.NotificationActivityType)
                :
                (PushNotificationManager.DefaultNotificationActivityType == null ? context.PackageManager.GetLaunchIntentForPackage(context.PackageName) : new Intent(Application.Context, PushNotificationManager.DefaultNotificationActivityType));

            Bundle extras = new Bundle();
            extras.PutString(nameof(title), title);
            extras.PutString(nameof(message), message);
            foreach (var p in parameters)
                extras.PutString(p.Key, p.Value.ToString());

            if (extras != null)
            {
                extras.PutInt(ActionNotificationIdKey, notifyId);
                resultIntent.PutExtras(extras);
            }

            if (PushNotificationManager.NotificationActivityFlags != null)
            {
                resultIntent.SetFlags(PushNotificationManager.NotificationActivityFlags.Value);
            }

            int requestCode = new Java.Util.Random().NextInt();
            var pendingIntent = PendingIntent.GetActivity(context, requestCode, resultIntent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context, chanId)
                .SetSmallIcon(context.ApplicationInfo.Icon)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent);
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.Notify(notifyId, notificationBuilder.Build());
        }
    }
}
