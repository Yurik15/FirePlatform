using System;
using System.IO;
using System.Xml.Serialization;
using FirePlatform.Mobile.Common.Entities;
using FirePlatform.Mobile.Common.Tools;

namespace FirePlatform.Mobile.Common.Implements
{
    public class ParserXml<TEntity> : IParser<TEntity> where TEntity : class, new()
    {
        public ParserXml()
        {
        }

        public TEntity Deserialize(string url)
        {
            TEntity deserializedObject = null;
            var content = FileHelper.DownloadFileBytes(url);
            if (content == null) return deserializedObject;

            var stream = new MemoryStream(content);
            var xmlSerializer = new XmlSerializer(typeof(TEntity));
            deserializedObject = (TEntity)xmlSerializer.Deserialize(stream);

            return deserializedObject;
        }

        public bool Serialize(TEntity itemGroup)
        {
            return true;
        }
    }
}
