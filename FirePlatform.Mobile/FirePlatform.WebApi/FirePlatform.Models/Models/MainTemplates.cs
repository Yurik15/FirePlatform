using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FirePlatform.Models.Models
{
    [Table("MainTemplates")]
    public class MainTemplates : IDomain
    {
        public string Lng { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public byte[] Data { get; set; }
    }
}
