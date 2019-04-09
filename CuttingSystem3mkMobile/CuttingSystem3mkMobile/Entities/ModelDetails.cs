using System;
using CuttingSystem3mkMobile.Enums;

namespace CuttingSystem3mkMobile.Entities
{
    public class ModelDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ModelSideEnum Side { get; set; }
        public byte[] FileData { get; set; }
        public byte[] ImageData { get; set; }
    }
}
