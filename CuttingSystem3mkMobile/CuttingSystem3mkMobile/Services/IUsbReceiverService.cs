using System;
namespace CuttingSystem3mkMobile.Services
{
    public interface IUsbReceiverService
    {
        event EventHandler<bool> OnDeviceAttach;
    }
}
