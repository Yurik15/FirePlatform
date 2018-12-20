using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using ItemControl = FirePlatform.Mobile.Common.Entities.Item;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Tools;

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
                    return PrepareTextControl(controlDetails);
                case ControlType.numeric:
                    return PrepareNumericControl(controlDetails);
                case ControlType.combo:
                    return PreparePicker(controlDetails);
            }
            var formula = new Label();
            formula.SetBinding(Label.TextProperty, "NumValueString", BindingMode.TwoWay);
            formula.BackgroundColor = Color.Azure;
            return formula;
        }
        private static View PrepareTextControl(ItemControl controlDetails)
        {
            var text = new Entry();
            text.BindingContext = controlDetails;
            text.SetBinding(Entry.TextProperty, nameof(controlDetails.NumValueString), BindingMode.TwoWay);
            text.Keyboard = Keyboard.Text;
            text.TextChanged += (object sender, TextChangedEventArgs e) => { ItemsControlLoader.Intance().RefreshForlumas(); };
            return text;
        }
        private static View PrepareNumericControl(ItemControl controlDetails)
        {
            var numeric = new Entry();
            numeric.BindingContext = controlDetails;
            numeric.SetBinding(Entry.TextProperty, nameof(controlDetails.NumValueString), BindingMode.TwoWay);
            numeric.Keyboard = Keyboard.Numeric;
            numeric.TextChanged += (object sender, TextChangedEventArgs e) => { ItemsControlLoader.Intance().RefreshForlumas(); }; ;
            return numeric;
        }
        private static View PreparePicker(ItemControl controlDetails)
        {
            var picker = new Picker();
            picker.ItemsSource = controlDetails.MultiItemDict.ToArray();
            picker.SetBinding(Picker.SelectedIndexProperty, nameof(controlDetails.SelectedIndex), BindingMode.TwoWay);
            picker.ItemDisplayBinding = new Binding("ComboItemTitle");
            picker.SelectedIndexChanged += (object sender, EventArgs e) => { ItemsControlLoader.Intance().RefreshForlumas(); }; ;
            return picker;
        }
    }
}
