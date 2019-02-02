using System;
using FirePlatform.Mobile.Common.PushNotification.Abstractions;

namespace FirePlatform.Mobile.Common.PushNotification
{
    public class CrossPushNotification
    {
        public static bool AppIsClosed { get; set; } = true;
        //static Lazy<IPushNotification> Implementation = null;
        static Lazy<IPushNotification> Implementation = null;

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IPushNotification Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        public static void SetImplementation(Lazy<IPushNotification> pushNotification)
        {
            Implementation = pushNotification;
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
