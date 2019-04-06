using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingSystem3mk.Models.Models
{
    [Table("DeviceModel")]
    public class DeviceModel : IDomain
    {
        public string Name { get; set; }
        public List<CutModel> CutModels { get; set; }
    }
}
