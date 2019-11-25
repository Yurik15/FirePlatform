using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FirePlatform.Models.Models
{
    [Table("UserTemplates")]
    public class UserTemplates : IDomain
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string MainName { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
