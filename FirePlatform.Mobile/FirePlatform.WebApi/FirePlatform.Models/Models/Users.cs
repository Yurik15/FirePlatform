using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FirePlatform.Models.Models
{
    [Table("Users")]
    public class Users : IDomain
    { 
        public string Login { get; set; }
        public string Password { get; set; }
        //public IEnumerable<UserForm> UserForms { get; set; }
        public IEnumerable<UserTemplates> UserTemplates { get; set; }
        //public IEnumerable<UserRole> UserRoles{ get; set; }

    }
}
