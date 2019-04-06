using System;
namespace FirePlatform.WebApi.Model.Template
{
    public class DataDependItem
    {
        private string _name;
        private string[] _arrayNames;

        public string Name
        {
            get => _name;
            set
            {
                _name = value?.Trim().ToLower() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(_name))
                {

                }
            }
        }
        /*public string[] ArrayNames
        {
            get => _arrayNames;
            set
            {
                _arrayNames = value;
            }
        }*/
        public bool IsVisibile { get; set; }
        public bool IsGhostFormula { get; set; }
        public Item ReferencedItem { get; set; }
    }
}
