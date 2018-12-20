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

        private const string PROP_LOGIN = "Login";
        private const string PROP_PASSWORD = "Password";
        private const string REMEMBER_ME = "RememberMe";
        private const string PROP_CULTURE_ID = "CultureId";

        public static string Login
        {
            get => AppSettings.GetValueOrDefault(PROP_LOGIN, "");
            set => AppSettings.AddOrUpdateValue(PROP_LOGIN, value);
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
        public static string Culture
        {
            get
            {
                return AppSettings.GetValueOrDefault(PROP_CULTURE_ID, "en");
            }
            set
            {
                AppSettings.AddOrUpdateValue(PROP_CULTURE_ID, value);
            }
        }
    }
}
