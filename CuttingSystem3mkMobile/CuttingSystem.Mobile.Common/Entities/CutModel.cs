namespace CuttingSystem.Mobile.Common.Entities
{
    public class CutModel
    {
        public string Name { get; set; }
        public string QRCode { get; set; }
        public int IdCutFile { get; set; }
        //public CutFile CutFile { get; set; }
        public int IdDeviceModel { get; set; }
        public DeviceModel DeviceModel { get; set; }
        //public List<CutCode> CutCodes { get; set; }
    }
}
