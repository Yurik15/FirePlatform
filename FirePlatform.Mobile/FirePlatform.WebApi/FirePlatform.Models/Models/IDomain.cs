using System.ComponentModel.DataAnnotations;

namespace FirePlatform.Models.Models
{
    public abstract class IDomain
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
