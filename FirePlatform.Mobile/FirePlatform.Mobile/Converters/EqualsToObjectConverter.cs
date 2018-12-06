using System;
using System.Globalization;
using Xamarin.Forms;

namespace FirePlatform.Mobile.Converters
{
    public class EqualsToObjectConverter<T> : IValueConverter
    {
        public T TrueObject { set; get; }

        public T FalseObject { set; get; }

        public object EqualsObject { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return FalseObject;
            return value.Equals(EqualsObject) ? TrueObject : FalseObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((T)value).Equals(TrueObject);
        }
    }
}
