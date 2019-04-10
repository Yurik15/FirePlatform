using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CuttingSystem3mkMobile.Entities;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.Controls
{
    public partial class DeviceControl : ContentView
    {
        public DeviceControl()
        {
            InitializeComponent();

            NameLbl.SetBinding(Label.TextProperty, new Binding(nameof(NameDevice), source: this));
            TapGestureCtrl.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(nameof(DeviceCommand), source: this));
            TapGestureCtrl.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding(nameof(DeviceCommandParameter), source: this));
        }

        public static readonly BindableProperty ImageByteArrayProperty = BindableProperty.Create(
            propertyName: nameof(ImageByteArray),
            returnType: typeof(byte[]),
            declaringType: typeof(DeviceControl),
            defaultValue: null,
            defaultBindingMode: BindingMode.OneWay);

        public byte[] ImageByteArray
        {
            get { return (byte[])GetValue(ImageByteArrayProperty); }
            set { SetValue(ImageByteArrayProperty, value); }
        }

        public static readonly BindableProperty NameDeviceProperty = BindableProperty.Create(
            propertyName: nameof(NameDevice),
            returnType: typeof(string),
            declaringType: typeof(DeviceControl),
            defaultBindingMode: BindingMode.OneWay);

        public string NameDevice
        {
            get { return (string)GetValue(NameDeviceProperty); }
            set { SetValue(NameDeviceProperty, value); }
        }


        public static readonly BindableProperty IsLoadingPdfProperty = BindableProperty.Create(
            propertyName: nameof(IsLoadingPdf),
            returnType: typeof(bool),
            declaringType: typeof(DeviceControl),
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsLoadingPdf
        {
            get { return (bool)GetValue(IsLoadingPdfProperty); }
            set { SetValue(IsLoadingPdfProperty, value); }
        }

        protected async override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(ImageByteArray))
            {
                await Task.Run(() =>
                {
                    var imageArray = ImageByteArray;
                    if (imageArray != null && imageArray.Length > 0)
                    {
                        this.IsLoadingPdf = true;

                        var stream = new MemoryStream(imageArray);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DeviceImg.Source = ImageSource.FromStream(() =>
                            {
                                return stream;
                            });
                        });
                    }
                    this.IsLoadingPdf = false;
                });
            }
            base.OnPropertyChanged(propertyName);
        }

        public static readonly BindableProperty DeviceCommandProperty = BindableProperty.Create(
            propertyName: nameof(DeviceCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(DeviceControl),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
            });

        public ICommand DeviceCommand
        {
            get { return (ICommand)GetValue(DeviceCommandProperty); }
            set { SetValue(DeviceCommandProperty, value); }
        }

        public static readonly BindableProperty DeviceCommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(DeviceCommandParameter),
            returnType: typeof(object),
            declaringType: typeof(DeviceControl),
            defaultBindingMode: BindingMode.TwoWay);


        public object DeviceCommandParameter
        {
            get { return (object)GetValue(DeviceCommandParameterProperty); }
            set { SetValue(DeviceCommandParameterProperty, value); }
        }

    }
}
