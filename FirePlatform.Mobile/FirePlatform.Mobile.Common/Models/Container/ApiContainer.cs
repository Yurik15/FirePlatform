using System;
using System.Collections.Generic;
using System.Text;

namespace FirePlatform.Mobile.Common.Models.Container
{
    public class ApiContainer<TEntity>
    {
        public IEnumerable<TEntity> DataCollection { get; set; }
        public TEntity DataObject { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
