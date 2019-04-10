using Android.Content;

namespace CuttingSystem3mkMobile.Droid
{
    public interface IUsbReceiver
    {
        void OnReceive(Context context, Intent intent);
    }
}