using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Model.Requests
{
    public class PreselectionRequest
    {
        public int UserId { get; set; }
        public bool PreselectionEnabled { get; set; }
    }
}
