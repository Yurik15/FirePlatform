using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirePlatform.Models.Models
{
    [Table("UserTemplates")]
    public class UserTemplates : IDomain
    {
        [Key]
        public override int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Users User { get; set; }
        public string MainName { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
