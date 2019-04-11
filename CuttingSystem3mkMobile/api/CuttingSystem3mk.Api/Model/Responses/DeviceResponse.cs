using CuttingSystem3mkMobile.Api.Model;
using System.Collections.Generic;

namespace CuttingSystem3mk.Api.Model
{
    public class DeviceResponse
    {
        public IEnumerable<DeviceDetails> Devices { get; set; }
    }
}
