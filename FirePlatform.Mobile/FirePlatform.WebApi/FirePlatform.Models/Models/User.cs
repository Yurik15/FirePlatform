using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FirePlatform.Models.Models
{
    [Table("User")]
    public class User : IDomain
    { 
        public string Login { get; set; }
        public string Password { get; set; }
        public IEnumerable<UserForm> UserForms { get; set; }
        //public IEnumerable<UserRole> UserRoles{ get; set; }

    }
}
