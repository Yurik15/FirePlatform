using CuttingSystem3mkMobile.Models.Models;
using System.Collections.Generic;

namespace CuttingSystem3mkMobile.Api.Model
{
    public class ModelDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdCutFile { get; set; }
        public byte[] FileData { get; set; }
        public byte[] ImageData { get; set; }
        public int IdDeviceModel { get; set; }
    }
}
