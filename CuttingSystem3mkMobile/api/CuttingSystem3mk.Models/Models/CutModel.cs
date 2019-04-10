using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingSystem3mkMobile.Models.Models
{
    [Table("CutModel")]
    public class CutModel : IDomain
    {
        public string Name { get; set; }
        /// <summary>
        /// token of barcode for cutting
        /// </summary>
        public string QRCode { get; set; }
        public int IdCutFile { get; set; }
        [ForeignKey("IdCutFile")]
        public CutFile CutFile { get; set; }
        public int IdDeviceModel { get; set; }
        [ForeignKey("IdDeviceModel")]
        public DeviceModel DeviceModel { get; set; }
        public List<CutCode> CutCodes { get; set; }
    }
}
