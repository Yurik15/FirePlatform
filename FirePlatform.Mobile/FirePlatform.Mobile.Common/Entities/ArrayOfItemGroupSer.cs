using System;
using System.Xml.Serialization;

namespace FirePlatform.Mobile.Common.Entities
{
    [XmlRoot("ArrayOfItemGroupSer")]
    public class ArrayOfItemGroupSer
    {
        [XmlElement(ElementName = "ItemGroupSer")]
        public ItemGroup ItemGroupSer { get;set;}
    }
}
