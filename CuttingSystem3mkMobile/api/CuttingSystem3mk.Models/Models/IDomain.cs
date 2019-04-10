using System.ComponentModel.DataAnnotations;

namespace CuttingSystem3mkMobile.Models.Models
{
    public abstract class IDomain
    {
        [Key]
        public int Id { get; set; }
    }
}
