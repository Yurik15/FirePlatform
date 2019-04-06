using System;
using System.Collections.Generic;
using System.Text;

namespace CuttingSystem3mk.Models.Containers.Infrastructure
{
    public abstract class BaseContainer<TEntity>
    {
        public IEnumerable<TEntity> DataCollection { get; set; }
        public TEntity DataObject { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
