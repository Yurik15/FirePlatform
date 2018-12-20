using System.ComponentModel.DataAnnotations;

namespace FirePlatform.Models.Models
{
    public abstract class IDomain
    {
        [Key]
        public int Id { get; set; }
    }
}
