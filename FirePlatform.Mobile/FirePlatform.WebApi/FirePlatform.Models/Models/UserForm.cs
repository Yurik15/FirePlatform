using System.ComponentModel.DataAnnotations.Schema;

namespace FirePlatform.Models.Models
{
    [Table("UserForm")]
    public class UserForm : IDomain
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public Users User { get; set; }
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public TemplateModel Form { get; set; }
    }
}
