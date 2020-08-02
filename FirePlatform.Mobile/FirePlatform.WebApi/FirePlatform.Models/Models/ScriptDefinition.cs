using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FirePlatform.Models.Models
{
    [Table("ScriptDefinitions")]
    public class ScriptDefinition : IDomain
    {
        public string Lng { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Link { get; set; }
    }
}
