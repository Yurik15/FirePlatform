using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePlatform.WebApi.Model.Template
{
    public class MatrixModel
    {
        private List<Item> items;

        public MatrixModel()
        {
            Items = new List<Item>();
        }
        public List<Item> Items { get => items ?? (items = new List<Item>()); set => items = value; }
    }
}
