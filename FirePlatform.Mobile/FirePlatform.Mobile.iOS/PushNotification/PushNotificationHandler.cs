using System;
using System.Collections.Generic;
using FirePlatform.Mobile.Common.PushNotification.Abstractions;
using Foundation;
using UIKit;

namespace FirePlatform.Mobile.iOS.PushNotification
{
    public class PushNotificationHandler : IPushNotificationHandler
    {
        public const string DomainTag = nameof(PushNotificationHandler);

        public void OnError(string error)
        {
            System.Diagnostics.Debug.WriteLine($"{DomainTag} - OnError - {error}");
        }

        public void OnOpened(NotificationResponse response)
        {
            System.Diagnostics.Debug.WriteLine($"{DomainTag} - OnOpened");
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
            System.Diagnostics.Debug.WriteLine($"{DomainTag} - OnReceived");

            var notification = new UILocalNotification();

            var userInfo = new NSMutableDictionary();
            foreach (var param in parameters)
            {
                userInfo.Add(new NSString(param.Key), new NSString(param.Value.ToString()));
            }

            notification.UserInfo = NSDictionary.FromDictionary(userInfo);
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(1);

            /*if (parameters.ContainsKey(NotificationFields.TitleField))
            {
                notification.AlertTitle = parameters[NotificationFields.TitleField].ToString();
            }
            if (parameters.ContainsKey(NotificationFields.BodyField))
            {
                notification.AlertBody = parameters[NotificationFields.BodyField].ToString();
            }
            if (parameters.ContainsKey(NotificationFields.BadgerField))
            {
                if (int.TryParse(parameters[NotificationFields.BadgerField].ToString(), out int result))
                {
                    notification.ApplicationIconBadgeNumber = result;
                }

            }*/
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}
