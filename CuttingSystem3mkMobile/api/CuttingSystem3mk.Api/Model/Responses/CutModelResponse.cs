using CuttingSystem3mkMobile.Models.Models;
using System.Collections.Generic;

namespace CuttingSystem3mkMobile.Api.Model.Responses
{
    public class CutModelResponse
    {
        public string Name { get; set; }
        public string QRCode { get; set; }
        public int IdCutFile { get; set; }
        public CutFile CutFile { get; set; }
        public int IdDeviceModel { get; set; }
        public DeviceModel DeviceModel { get; set; }
        public List<CutCode> CutCodes { get; set; }
    }
}
