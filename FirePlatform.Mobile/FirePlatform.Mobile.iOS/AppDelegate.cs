using System;
using System.Collections.Generic;
using System.Linq;
using FirePlatform.Mobile.iOS.PushNotification;
using Foundation;
using Syncfusion.ListView.XForms.iOS;
using UIKit;
using UserNotifications;
using FirePlatform.Mobile.Common.PushNotification.Abstractions;

namespace FirePlatform.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Calabash.Start();
            SfListViewRenderer.Init();
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
        #region PushNotification

        private async void FinishedLaunchingAsync(UIApplication app, NSDictionary options)
        {
            await PushNotificationManager.Initialize(options, new Lazy<IPushNotification>(() => { return new PushNotificationManager(); }), true);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            PushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            PushNotificationManager.DidReceiveMessage(userInfo);
        }
        #endregion PushNotification
    }
}
