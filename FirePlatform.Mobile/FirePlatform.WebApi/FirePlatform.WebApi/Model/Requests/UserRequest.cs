using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Model.Requests
{
    public class UserRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
