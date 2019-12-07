
using System;

namespace FirePlatform.WebApi.Model.Template
{
    [Serializable]
    public class Picture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NonSerialized]
        public string Data;
        [NonSerialized]
        public bool ToFetch;
    }
}
