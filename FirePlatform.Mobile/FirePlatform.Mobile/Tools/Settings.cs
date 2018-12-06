using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace FirePlatform.Mobile.Tools
{
    public class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        private const string PROP_USER_NAME = "UserName";
        private const string PROP_PASSWORD = "Password";
        private const string REMEMBER_ME = "RememberMe";

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(PROP_USER_NAME, "");
            set => AppSettings.AddOrUpdateValue(PROP_USER_NAME, value);
        }

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(PROP_PASSWORD, "");
            set => AppSettings.AddOrUpdateValue(PROP_PASSWORD, value);
        }

        public static bool RememberMe
        {
            get => AppSettings.GetValueOrDefault(REMEMBER_ME, false);
            set => AppSettings.AddOrUpdateValue(REMEMBER_ME, value);
        }
    }
}
