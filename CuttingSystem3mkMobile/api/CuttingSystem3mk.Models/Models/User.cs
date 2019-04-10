using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingSystem3mkMobile.Models.Models
{
    [Table("User")]
    public class User : IDomain
    { 
        public string Login { get; set; }
        public string Password { get; set; }
        //public IEnumerable<UserRole> UserRoles{ get; set; }

    }
}
