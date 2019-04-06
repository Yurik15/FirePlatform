using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingSystem3mk.Models.Models
{
    [Table("CutFile")]
    public class CutFile : IDomain
    {
        public byte[] Value { get; set; }
    }
}
