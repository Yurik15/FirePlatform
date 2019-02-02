﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirePlatform.Mobile.Common.PushNotification.Abstractions
{
    public enum NotificationActionType
    {
        Default,
        AuthenticationRequired, //Only applies for iOS
        Foreground,
        Destructive  //Only applies for iOS
    }

    public enum PushNotificationErrorType
    {
        Unknown,
        PermissionDenied,
        RegistrationFailed,
        UnregistrationFailed
    }

    public enum NotificationCategoryType
    {
        Default,
        Custom,
        Dismiss
    }

    public delegate void PushNotificationTokenEventHandler(object source, PushNotificationTokenEventArgs e);

    public class PushNotificationTokenEventArgs : EventArgs
    {
        public string Token { get; }

        public PushNotificationTokenEventArgs(string token)
        {
            Token = token;
        }

    }

    public delegate void PushNotificationErrorEventHandler(object source, PushNotificationErrorEventArgs e);

    public class PushNotificationErrorEventArgs : EventArgs
    {
        public PushNotificationErrorType Type;
        public string Message { get; }

        public PushNotificationErrorEventArgs(PushNotificationErrorType type, string message)
        {
            Type = type;
            Message = message;
        }

    }

    public delegate void PushNotificationDataEventHandler(object source, PushNotificationDataEventArgs e);

    public class PushNotificationDataEventArgs : EventArgs
    {
        public IDictionary<string, object> Data { get; }

        public PushNotificationDataEventArgs(IDictionary<string, object> data)
        {
            Data = data;
        }

    }

    public delegate void PushNotificationResponseEventHandler(object source, PushNotificationResponseEventArgs e);

    public class PushNotificationResponseEventArgs : EventArgs
    {
        public string Identifier { get; }

        public IDictionary<string, object> Data { get; }

        public NotificationCategoryType Type { get; }

        public PushNotificationResponseEventArgs(IDictionary<string, object> data, string identifier = "", NotificationCategoryType type = NotificationCategoryType.Default)
        {
            Identifier = identifier;
            Data = data;
            Type = type;
        }

    }

    /// <summary>
    /// Interface for PushNotification
    /// </summary>
    public interface IPushNotification
    {
        /// <summary>
        /// Notification handler to receive, customize notification feedback and provide user actions
        /// </summary>
        IPushNotificationHandler NotificationHandler { get; set; }
        /// <summary>
        /// Event triggered when token is refreshed
        /// </summary>
        event PushNotificationTokenEventHandler OnTokenRefresh;
        /// <summary>
        /// Event triggered when a notification is opened
        /// </summary>
        event PushNotificationResponseEventHandler OnNotificationOpened;
        /// <summary>
        /// Event triggered when a notification is received
        /// </summary>
        event PushNotificationDataEventHandler OnNotificationReceived;
        /// <summary>
        /// Event triggered when a notification is deleted (Android Only)
        /// </summary>
        event PushNotificationDataEventHandler OnNotificationDeleted;
        /// <summary>
        /// Event triggered when there's an error
        /// </summary>
        event PushNotificationErrorEventHandler OnNotificationError;
        /// <summary>
        /// Register push notifications on demand
        /// </summary>
        /// <returns></returns>
        Task RegisterForPushNotifications();
        /// <summary>
        /// Unregister push notifications on demand
        /// </summary>
        /// <returns></returns>
        string Token { get; }
    }
}
