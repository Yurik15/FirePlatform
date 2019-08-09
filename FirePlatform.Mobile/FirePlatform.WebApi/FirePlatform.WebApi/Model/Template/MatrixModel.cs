using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Model.Template
{
    public class MatrixModel
    {
        public MatrixModel()
        {
            Items = new List<Item>();
        }
        public List<Item> Items { get; set; }
    }
}
