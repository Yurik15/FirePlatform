using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingSystem3mkMobile.Models.Models
{
    [Table("CutCode")]
    public class CutCode : IDomain
    {
        public string Barcode { get; set; }
        public bool IsActive { get; set; }
        public int? IdCutModel { get; set; }
        [ForeignKey("IdCutModel")]
        public CutModel CutModel { get; set; }
    }
}
