using System;
using CuttingSystem3mkMobile.Models;

namespace CuttingSystem3mkMobile.Services
{
    public interface IPrintManager
    {
        DeviceInfo[] Devices();
        void ConnectAndSend(byte[] file, int prodId, int vendorId);
    }
}
