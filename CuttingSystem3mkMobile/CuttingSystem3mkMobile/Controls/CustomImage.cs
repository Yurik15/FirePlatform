using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.Controls
{
    public class CustomImage : Image
    {
        public static readonly BindableProperty ImageArrayBytesProperty = BindableProperty.Create(
            propertyName: nameof(ImageArrayBytes),
            returnType: typeof(byte[]),
            declaringType: typeof(CustomImage),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue != null && newValue is byte[] arrayBytes && arrayBytes.Any() && bindable is CustomImage customImage)
                {
                    var stream = new MemoryStream(arrayBytes);
                    customImage.Source = ImageSource.FromStream(() => stream);
                }
            });
        public byte[] ImageArrayBytes
        {
            get { return (byte[])GetValue(ImageArrayBytesProperty); }
            set { SetValue(ImageArrayBytesProperty, value); }
        }
    }
}
