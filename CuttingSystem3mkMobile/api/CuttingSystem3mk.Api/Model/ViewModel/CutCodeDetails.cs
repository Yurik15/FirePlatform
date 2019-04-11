namespace CuttingSystem3mk.Api.Model
{
    public class CutCodeDetails
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public bool IsActive { get; set; }
        public int IdCutModel { get; set; }
    }
}
