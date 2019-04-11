using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingSystem3mkMobile.Models.Models
{
    [Table("CutFile")]
    public class CutFile : IDomain
    {
        public string Value { get; set; }
        public string ImageValue { get; set; }
    }
}
