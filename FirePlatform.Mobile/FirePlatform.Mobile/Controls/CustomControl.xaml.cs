using System;
using System.Collections.Generic;
using FirePlatform.Mobile.Enums;
using Xamarin.Forms;
using ItemControl = FirePlatform.Mobile.Common.Entities.Item;

namespace FirePlatform.Mobile.Controls
{
    public partial class CustomControl : ContentView
    {
        public CustomControl()
        {
            InitializeComponent();
        }

        #region [Bindable Property]
        public ItemControl Item
        {
            get
            {
                return (ItemControl)GetValue(ItemProperty);
            }
            set
            {
                SetValue(ItemProperty, value);
            }
        }

        public static readonly BindableProperty ItemProperty = BindableProperty.Create(
            propertyName: nameof(CurrentControlType),
            returnType: typeof(ItemControl),
            declaringType: typeof(CustomControl),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);


        public ControlType CurrentControlType
        {
            get
            {
                return (ControlType)GetValue(CurrentControlTypeProperty);
            }
            set
            {
                SetValue(CurrentControlTypeProperty, value);
            }
        }

        public static readonly BindableProperty CurrentControlTypeProperty = BindableProperty.Create(
            propertyName: nameof(CurrentControlType),
            returnType: typeof(ControlType),
            declaringType: typeof(CustomControl),
            defaultValue: ControlType.undefined,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: HandleBindingPropertyChangedDelegate);

        public static void HandleBindingPropertyChangedDelegate(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as CustomControl;

            ControlType oldVal = (ControlType)oldValue;
            ControlType newVal = (ControlType)newValue;

            if (oldVal != newVal)
            {
                control.Content = ReturnView(control.Item, newVal);
            }
        }
        #endregion

        public static View ReturnView(ItemControl controlDetails, ControlType controlType)
        {
            switch (controlType)
            {
                case ControlType.text:
                    return new Entry() { Text = controlDetails.NumValue.ToString(), Keyboard = Keyboard.Text };
                case ControlType.numeric:
                    return new Entry() { Text = controlDetails.NumValue.ToString(), Keyboard = Keyboard.Numeric };
            }
            return new Label()
            {
                Text = controlDetails.NumValue.ToString(),
                BackgroundColor = Color.Azure,
                TextColor = Color.Black
            };
        }
    }
}
