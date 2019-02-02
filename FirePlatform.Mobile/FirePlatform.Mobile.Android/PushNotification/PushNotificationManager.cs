using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Firebase.Iid;
using FirePlatform.Mobile.Common.PushNotification;
using FirePlatform.Mobile.Common.PushNotification.Abstractions;

namespace FirePlatform.Mobile.Droid.PushNotification
{
    public class PushNotificationManager : IPushNotification
    {
        //internal static PushNotificationActionReceiver ActionReceiver = null;
        static NotificationResponse delayedNotificationResponse = null;
        internal const string KeyGroupName = "PushNotification";
        internal const string TokenKey = "Token";
        internal const string AppVersionCodeKey = "AppVersionCodeKey";
        internal const string AppVersionNameKey = "AppVersionNameKey";
        internal const string AppVersionPackageNameKey = "AppVersionPackageNameKey";
        public static Type NotificationActivityType { get; set; }
        public static ActivityFlags? NotificationActivityFlags { get; set; } = ActivityFlags.ClearTop | ActivityFlags.SingleTop;
        public static string DefaultNotificationChannelId { get; set; } = "PushNotificationChannel";
        public static string DefaultNotificationChannelName { get; set; } = "General";

        internal static Type DefaultNotificationActivityType { get; set; } = null;

        static Context _context;

        #region implementation IPushNotification


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


        static PushNotificationResponseEventHandler _onNotificationOpened;
        public event PushNotificationResponseEventHandler OnNotificationOpened
        {
            add
            {
                var previousVal = _onNotificationOpened;
                _onNotificationOpened += value;
                if (delayedNotificationResponse != null && previousVal == null)
                {
                    var tmpParams = delayedNotificationResponse;
                    _onNotificationOpened?.Invoke(CrossPushNotification.Current, new PushNotificationResponseEventArgs(tmpParams.Data, tmpParams.Identifier, tmpParams.Type));
                    delayedNotificationResponse = null;
                }
            }
            remove
            {
                _onNotificationOpened -= value;
            }
        }

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

        public string Token { get { return Android.App.Application.Context.GetSharedPreferences(KeyGroupName, FileCreationMode.Private).GetString(TokenKey, string.Empty); } }

        public IPushNotificationHandler NotificationHandler { get; set; }

        public async System.Threading.Tasks.Task RegisterForPushNotifications()
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                var token = FirebaseInstanceId.Instance.Token;
                if (!string.IsNullOrEmpty(token))
                {
                    SaveToken(token);
                }
            });

        }

        #endregion implementation IPushNotification


        public static void Initialize(Context context, bool resetToken, Lazy<IPushNotification> pushNotification, bool createDefaultNotificationChannel = true, bool autoRegistration = true)
        {
            CrossPushNotification.SetImplementation(pushNotification);
            _context = context;

            CrossPushNotification.Current.NotificationHandler = CrossPushNotification.Current.NotificationHandler ?? new PushNotificationHandler();
            if (autoRegistration)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {

                    var packageName = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, PackageInfoFlags.MetaData).PackageName;
                    var versionCode = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, PackageInfoFlags.MetaData).VersionCode;
                    var versionName = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, PackageInfoFlags.MetaData).VersionName;
                    var prefs = Android.App.Application.Context.GetSharedPreferences(PushNotificationManager.KeyGroupName, FileCreationMode.Private);

                    try
                    {

                        var storedVersionName = prefs.GetString(PushNotificationManager.AppVersionNameKey, string.Empty);
                        var storedVersionCode = prefs.GetString(PushNotificationManager.AppVersionCodeKey, string.Empty);
                        var storedPackageName = prefs.GetString(PushNotificationManager.AppVersionPackageNameKey, string.Empty);


                        if (resetToken || (!string.IsNullOrEmpty(storedPackageName) && (!storedPackageName.Equals(packageName, StringComparison.CurrentCultureIgnoreCase) || !storedVersionName.Equals(versionName, StringComparison.CurrentCultureIgnoreCase) || !storedVersionCode.Equals($"{versionCode}", StringComparison.CurrentCultureIgnoreCase))))
                        {
                            CleanUp();

                        }

                    }
                    catch (Exception ex)
                    {
                        _onNotificationError?.Invoke(CrossPushNotification.Current, new PushNotificationErrorEventArgs(PushNotificationErrorType.UnregistrationFailed, ex.ToString()));
                    }
                    finally
                    {
                        var editor = prefs.Edit();
                        editor.PutString(PushNotificationManager.AppVersionNameKey, $"{versionName}");
                        editor.PutString(PushNotificationManager.AppVersionCodeKey, $"{versionCode}");
                        editor.PutString(PushNotificationManager.AppVersionPackageNameKey, $"{packageName}");
                        editor.Commit();
                    }


                    CrossPushNotification.Current.RegisterForPushNotifications();


                });
            }


            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O && createDefaultNotificationChannel)
            {
                // Create channel to show notifications.
                string channelId = DefaultNotificationChannelId;
                string channelName = DefaultNotificationChannelName;
                NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

                notificationManager.CreateNotificationChannel(new NotificationChannel(channelId,
                    channelName, NotificationImportance.Default));
            }
        }


        public static void ProcessIntent(Activity activity, Intent intent, bool enableDelayedResponse = true)
        {
            DefaultNotificationActivityType = activity.GetType();

            Bundle extras = intent?.Extras;
            if (extras != null && !extras.IsEmpty)
            {
                var parameters = new Dictionary<string, object>();
                foreach (var key in extras.KeySet())
                {
                    if (!parameters.ContainsKey(key) && extras.Get(key) != null)
                        parameters.Add(key, $"{extras.Get(key)}");
                }

                NotificationManager manager = _context.GetSystemService(Context.NotificationService) as NotificationManager;
                var notificationId = extras.GetInt(PushNotificationHandler.ActionNotificationIdKey, -1);
                if (notificationId != -1)
                {
                    var notificationTag = extras.GetString(PushNotificationHandler.ActionNotificationTagKey, string.Empty);
                    if (notificationTag == null)
                        manager.Cancel(notificationId);
                    else
                        manager.Cancel(notificationTag, notificationId);
                }


                var response = new NotificationResponse(parameters, extras.GetString(PushNotificationHandler.ActionIdentifierKey, string.Empty));

                if (_onNotificationOpened == null && enableDelayedResponse)
                    delayedNotificationResponse = response;
                else
                    _onNotificationOpened?.Invoke(CrossPushNotification.Current, new PushNotificationResponseEventArgs(response.Data, response.Identifier, response.Type));

                CrossPushNotification.Current.NotificationHandler?.OnOpened(response);
            }
        }



        public void UnregisterForPushNotifications()
        {
            Reset();
        }

        public static void Reset()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    CleanUp();
                });
            }
            catch (Exception ex)
            {
                _onNotificationError?.Invoke(CrossPushNotification.Current, new PushNotificationErrorEventArgs(PushNotificationErrorType.UnregistrationFailed, ex.ToString()));
            }


        }

        static void CleanUp()
        {
            Firebase.Iid.FirebaseInstanceId.Instance.DeleteInstanceId();
            SaveToken(string.Empty);
        }

        public void ClearAllNotifications()
        {
            NotificationManager manager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
            manager.CancelAll();
        }

        #region internal methods
        //Raises event for push notification token refresh
        internal static void RegisterToken(string token)
        {
            _onTokenRefresh?.Invoke(CrossPushNotification.Current, new PushNotificationTokenEventArgs(token));
        }
        internal static void RegisterData(IDictionary<string, object> data)
        {
            _onNotificationReceived?.Invoke(CrossPushNotification.Current, new PushNotificationDataEventArgs(data));
        }
        internal static void RegisterDelete(IDictionary<string, object> data)
        {
            _onNotificationDeleted?.Invoke(CrossPushNotification.Current, new PushNotificationDataEventArgs(data));
        }
        internal static void SaveToken(string token)
        {
            var editor = Android.App.Application.Context.GetSharedPreferences(PushNotificationManager.KeyGroupName, FileCreationMode.Private).Edit();
            editor.PutString(PushNotificationManager.TokenKey, token);
            editor.Commit();
        }
        #endregion internal methods
    }
}
