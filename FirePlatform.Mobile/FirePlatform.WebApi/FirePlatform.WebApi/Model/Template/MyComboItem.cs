using System;
namespace FirePlatform.WebApi.Model
{
    public class MyComboItem
    {
        public string ComboItemTitle { get; set; }
        public string ComboItemTag { get; set; }
        public bool IsVisibile
        {
            get
            {
                if (!string.IsNullOrEmpty(ComboItemTag))
                {
                    var splited = ComboItemTag.Split(',');
                    if (bool.TryParse(splited[1], out bool res))
                        return res;
                }
                return false;
            }
        }
    }
}
