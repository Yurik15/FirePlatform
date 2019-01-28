using System;
using System.Xml.Serialization;

namespace FirePlatform.WebApi.Model
{
    [XmlRoot("ArrayOfItemGroupSer")]
    public class ArrayOfItemGroupSer
    {
        [XmlElement(ElementName = "ItemGroupSer")]
        public ItemGroup[] ItemGroupSer { get; set; }
    }
}
