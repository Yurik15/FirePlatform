using System;
using System.Collections.Generic;
using System.Globalization;
using FirePlatform.Mobile.Controls;
using FirePlatform.Mobile.Enums;
using Xamarin.Forms;

namespace FirePlatform.Mobile.Converters
{
    public class ControlTypeConverter : IValueConverter
    {
        public ControlTypeConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var res = value.ToString().ToLower();
            if (dictionary.ContainsKey(res))
            {
                return dictionary[res];
            }
            return ControlType.text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return dictionary2[(ControlType)value];
        }

        Dictionary<string, ControlType> dictionary = new Dictionary<string, ControlType>()
        {
            {"num", ControlType.numeric },
            {"text", ControlType.text }
        };
        Dictionary<ControlType, string> dictionary2 = new Dictionary<ControlType, string>()
        {
            {ControlType.numeric, "num" },
            {ControlType.text, "text" }
        };
    }
}
