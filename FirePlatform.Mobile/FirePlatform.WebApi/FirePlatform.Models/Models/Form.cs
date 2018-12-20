using System;
using System.Collections.Generic;
using System.Text;

namespace FirePlatform.Models.Models
{
    public class Form : IDomain
    {
        public string Name { get; set; }
        public IEnumerable<UserForm> UserForms { get; set; }
    }
}
