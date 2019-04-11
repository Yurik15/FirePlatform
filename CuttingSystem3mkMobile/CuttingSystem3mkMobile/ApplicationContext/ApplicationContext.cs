using System;
namespace CuttingSystem3mkMobile.ApplicationContext
{
    public static class ApplicationContext
    {
        public static string AppToken
        {
            get; set;
        }
        public static string BaseUrl
        {
            get; set;
        }

        private static bool deviceConnected;
        public static event EventHandler<bool> OnDeviceAttach;
        public static bool DeviceConnected
        {
            get => deviceConnected;
            set
            {
                deviceConnected = value;
                OnDeviceAttach?.Invoke(null, value);
            }
        }
    }
}
