using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Tools;
using System.Collections.ObjectModel;
using FirePlatform.Mobile.Common.Entities;

namespace FirePlatform.Mobile.Controls
{
    public partial class CustomControl : ContentView
    {
        public CustomControl()
        {
            InitializeComponent();
        }

        #region [Bindable Property]

        public ObservableCollection<Item> CollectionItems
        {
            get
            {
                return (ObservableCollection<Item>)GetValue(CollectionItemsProperty);
            }
            set
            {
                SetValue(CollectionItemsProperty, value);
            }
        }

        public static readonly BindableProperty CollectionItemsProperty = BindableProperty.Create(
            propertyName: nameof(CollectionItems),
            returnType: typeof(ObservableCollection<Item>),
            declaringType: typeof(CustomControl),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);


        public Item Item
        {
            get
            {
                return (Item)GetValue(ItemProperty);
            }
            set
            {
                SetValue(ItemProperty, value);
            }
        }

        public static readonly BindableProperty ItemProperty = BindableProperty.Create(
            propertyName: nameof(CurrentControlType),
            returnType: typeof(Item),
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
            var items = control.CollectionItems;

            if (oldVal != newVal)
            {
                control.Item.ParametersDependToItem = items;
                control.Content = control.ReturnView(control.Item, newVal);
            }
        }
        #endregion

        public View ReturnView(Item controlDetails, ControlType controlType)
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
        private void Notify(Item item)
        {
            if (item != null)
            {
                if (item.ItemsToNeedNotify != null)
                {
                    foreach (var itemNotify in item.ItemsToNeedNotify)
                    {
                        itemNotify.Update();
                    }
                }
            }
        }
        private View PrepareTextControl(Item controlDetails)
        {
            var text = new Entry();
            text.BindingContext = controlDetails;
            text.SetBinding(Entry.TextProperty, nameof(controlDetails.NumValueString), BindingMode.TwoWay);
            text.Keyboard = Keyboard.Text;
            text.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                if (sender is Entry entry && entry.BindingContext is Item item)
                {
                    Notify(item);
                }
            };
            return text;
        }
        private View PrepareNumericControl(Item controlDetails)
        {
            var numeric = new Entry();
            numeric.BindingContext = controlDetails;
            numeric.SetBinding(Entry.TextProperty, nameof(controlDetails.NumValueString), BindingMode.TwoWay);
            numeric.Keyboard = Keyboard.Numeric;
            numeric.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                if (sender is Entry entry && entry.BindingContext is Item item)
                {
                    Notify(item);
                }
            };
            return numeric;
        }
        private View PreparePicker(Item controlDetails)
        {
            var picker = new Picker();
            picker.ItemsSource = controlDetails.MultiItemDict.ToArray();
            picker.SetBinding(Picker.SelectedIndexProperty, nameof(controlDetails.SelectedIndex), BindingMode.TwoWay);
            picker.ItemDisplayBinding = new Binding("ComboItemTitle");
            picker.SelectedIndexChanged += (object sender, EventArgs e) =>
            {
                if (sender is Picker pickerCtr && pickerCtr.BindingContext is Item item)
                {
                    Notify(item);
                }
            };
            return picker;
        }
    }
}
