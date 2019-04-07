using System;
using System.Globalization;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.Convertors
{
    public class InvertedBooleanConvertor : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        #endregion Public Methods
    }
}
