using System;
using FirePlatform.Mobile.Common.Entities;

namespace FirePlatform.Mobile.Common
{
    public interface IParser<TEntity> where TEntity : class, new()
    {
        bool Serialize(TEntity itemGroup);
        TEntity Deserialize(string url);
    }
}
