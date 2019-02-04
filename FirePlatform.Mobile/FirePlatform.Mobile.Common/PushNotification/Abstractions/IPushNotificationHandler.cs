using System;
namespace FirePlatform.Mobile.Common.PushNotification.Abstractions
{
    public interface IPushNotificationHandler
    {
        //Method triggered when an error occurs
        void OnError(string error);
        //Method triggered when a notification is opened
        void OnOpened(NotificationResponse response);
        //Method triggered when a notification is received
        void OnReceived(System.Collections.Generic.IDictionary<string, object> parameters);
    }
}
