using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.CloudMessaging;
using Firebase.InstanceID;
using FirePlatform.Mobile.Common.PushNotification;
using FirePlatform.Mobile.Common.PushNotification.Abstractions;
using Foundation;
using UIKit;
using UserNotifications;

namespace FirePlatform.Mobile.iOS.PushNotification
{
    public class PushNotificationManager : NSObject, IPushNotification, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        const string TokenKey = "Token";


        #region implementation IPushNotification
        static PushNotificationTokenEventHandler _onTokenRefresh;
        public event PushNotificationTokenEventHandler OnTokenRefresh
        {
            add
            {
                _onTokenRefresh += value;
            }
            remove
            {
                _onTokenRefresh -= value;
            }
        }

        static PushNotificationDataEventHandler _onNotificationDeleted;
        public event PushNotificationDataEventHandler OnNotificationDeleted
        {
            add
            {
                _onNotificationDeleted += value;
            }
            remove
            {
                _onNotificationDeleted -= value;
            }
        }

        static PushNotificationErrorEventHandler _onNotificationError;
        public event PushNotificationErrorEventHandler OnNotificationError
        {
            add
            {
                _onNotificationError += value;
            }
            remove
            {
                _onNotificationError -= value;
            }
        }

        static PushNotificationResponseEventHandler _onNotificationOpened;
        public event PushNotificationResponseEventHandler OnNotificationOpened
        {
            add
            {
                _onNotificationOpened += value;
            }
            remove
            {
                _onNotificationOpened -= value;
            }
        }


        static PushNotificationDataEventHandler _onNotificationReceived;
        public event PushNotificationDataEventHandler OnNotificationReceived
        {
            add
            {
                _onNotificationReceived += value;
            }
            remove
            {
                _onNotificationReceived -= value;
            }
        }

        public string Token { get { return string.IsNullOrEmpty(Messaging.SharedInstance.FcmToken) ? (NSUserDefaults.StandardUserDefaults.StringForKey(TokenKey) ?? string.Empty) : Messaging.SharedInstance.FcmToken; } }

        public IPushNotificationHandler NotificationHandler { get; set; }

        public async Task RegisterForPushNotifications()
        {
            var permisionTask = new TaskCompletionSource<bool>();

            Messaging.SharedInstance.Delegate = CrossPushNotification.Current as IMessagingDelegate;

            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge;

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = CrossPushNotification.Current as IUNUserNotificationCenterDelegate;

                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    if (error != null)
                        _onNotificationError?.Invoke(CrossPushNotification.Current, new PushNotificationErrorEventArgs(PushNotificationErrorType.PermissionDenied, error.Description));
                    else if (!granted)
                        _onNotificationError?.Invoke(CrossPushNotification.Current, new PushNotificationErrorEventArgs(PushNotificationErrorType.PermissionDenied, "Push notification permission not granted"));


                    permisionTask.SetResult(granted);
                });

            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

                permisionTask.SetResult(true);
            }


            var permissonGranted = await permisionTask.Task;

            if (permissonGranted)
            {
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
        }


        #endregion implementation IPushNotification



        public static async Task Initialize(NSDictionary options, Lazy<IPushNotification> pushNotification, bool autoRegistration = true)
        {
            Firebase.Core.App.Configure();
            CrossPushNotification.SetImplementation(pushNotification);
            CrossPushNotification.Current.NotificationHandler = CrossPushNotification.Current.NotificationHandler ?? new PushNotificationHandler();

            if (autoRegistration)
            {
                await CrossPushNotification.Current.RegisterForPushNotifications();
            }
        }

        public static void RemoteNotificationRegistrationFailed(NSError error)
        {
            System.Diagnostics.Debug.WriteLine("RemoteNotificationRegistrationFailed");

            _onNotificationError?.Invoke(CrossPushNotification.Current, new PushNotificationErrorEventArgs(PushNotificationErrorType.RegistrationFailed, error.Description));
        }

        public static void DidReceiveMessage(NSDictionary data)
        {
            System.Diagnostics.Debug.WriteLine("DidReceivedMessage");

            var parameters = GetParameters(data);
            _onNotificationReceived?.Invoke(CrossPushNotification.Current, new PushNotificationDataEventArgs(parameters));
        }


        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            System.Diagnostics.Debug.WriteLine("WillPresentNotification");

            completionHandler(UNNotificationPresentationOptions.Badge | UNNotificationPresentationOptions.Alert);
        }

        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            System.Diagnostics.Debug.WriteLine("DidReceiveNotificationResponse");

            var parameters = GetParameters(response.Notification.Request.Content.UserInfo);

            NotificationCategoryType catType = NotificationCategoryType.Default;
            if (response.IsCustomAction)
                catType = NotificationCategoryType.Custom;
            else if (response.IsDismissAction)
                catType = NotificationCategoryType.Dismiss;

            var notificationResponse = new NotificationResponse(parameters, string.Empty, catType);
            _onNotificationOpened?.Invoke(this, new PushNotificationResponseEventArgs(notificationResponse.Data, notificationResponse.Identifier, notificationResponse.Type));

            completionHandler();
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string token)
        {
            var refreshedToken = token;
            if (!string.IsNullOrEmpty(refreshedToken))
            {
                _onTokenRefresh?.Invoke(CrossPushNotification.Current, new PushNotificationTokenEventArgs(refreshedToken));
            }
            NSUserDefaults.StandardUserDefaults.SetString(token, TokenKey);
        }

        public void ClearAllNotifications()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RemoveAllDeliveredNotifications();
            }
            else
            {
                UIApplication.SharedApplication.CancelAllLocalNotifications();
            }
        }

        public void UnregisterForPushNotifications()
        {
            Messaging.SharedInstance.ShouldEstablishDirectChannel = false;

            UIApplication.SharedApplication.UnregisterForRemoteNotifications();
            NSUserDefaults.StandardUserDefaults.SetString(string.Empty, Token);
            InstanceId.SharedInstance.DeleteId((h) => { });
        }

        #region helpers

        private static IDictionary<string, object> GetParameters(NSDictionary data)
        {
            var parameters = new Dictionary<string, object>();
            foreach (var val in data)
            {
                parameters.Add($"{val.Key}", $"{val.Value}");
            }
            return parameters;
        }

        #endregion helpers
    }
}
