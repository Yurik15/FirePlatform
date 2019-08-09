using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Model.Requests
{
    public class UserRequest
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
