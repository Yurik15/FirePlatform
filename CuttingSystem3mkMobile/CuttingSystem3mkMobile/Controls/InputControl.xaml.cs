using System;
using System.Collections.Generic;
using CuttingSystem3mkMobile.Convertors;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.Controls
{
    public partial class InputControl : ContentView
    {
        public InputControl()
        {
            InitializeComponent();
            EntryCtr.SetBinding(Entry.AutomationIdProperty, new Binding(nameof(EntryAutomationID), source: this));
            EntryCtr.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
            EntryCtr.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this));
            EntryCtr.SetBinding(Entry.StyleProperty, new Binding(nameof(EntryStyle), source: this));
            EntryCtr.SetBinding(Entry.IsPasswordProperty, new Binding(nameof(IsPassword), source: this));
            EntryCtr.SetBinding(Entry.IsFocusedProperty, new Binding(nameof(IsFocused), source: this));
            EntryCtr.SetBinding(Entry.KeyboardProperty, new Binding(nameof(KeyboardType), source: this));
            EntryCtr.SetBinding(Entry.MaxLengthProperty, new Binding(nameof(MaxLength), source: this));
            EntryCtr.SetBinding(Entry.BehaviorsProperty, new Binding(nameof(BehaviorsEntry), source: this));
            EntryCtr.SetBinding(Entry.IsEnabledProperty, new Binding(nameof(IsEnabledEntry), source: this));
            EntryCtr.BorderColor = EntryCtr.InactiveColor;

            TitleLbl.SetBinding(Label.TextProperty, new Binding(nameof(Placeholder), source: this));
            TitleLbl.SetBinding(Label.StyleProperty, new Binding(nameof(LabelStyle), source: this));
            TitleLbl.SetBinding(Label.TextColorProperty, new Binding(nameof(EntryCtr.BorderColor), source: EntryCtr));

            ErrorLbl.SetBinding(Label.TextProperty, new Binding(nameof(ErrorText), source: this));
            ErrorLbl.SetBinding(Label.IsVisibleProperty, new Binding(nameof(ErrorText), converter: new StringEmptyToBoolConverter(), source: this));

            ErroImg.SetBinding(Image.IsVisibleProperty, new Binding(nameof(ErrorLbl.IsVisible), source: ErrorLbl));

            DescriptionCtrl.SetBinding(StackLayout.IsVisibleProperty, new Binding(nameof(ErrorLbl.IsVisible), converter: new InvertedBooleanConvertor(), source: ErrorLbl));
            DescriptionFirstRowLbl.SetBinding(Label.TextProperty, new Binding(nameof(DescriptionLine1), source: this));
            DescriptionSecondRowLbl.SetBinding(Label.TextProperty, new Binding(nameof(DescriptionLine2), source: this));
            DescriptionSecondRowLbl.SetBinding(Label.IsVisibleProperty, new Binding(nameof(EntryCtr.IsEnabled), source: EntryCtr));
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                ErrorText = string.Empty;
                if (_isFocused != value && IsVisibleTopLabel)
                {
                    if (value)
                    {
                        TitleLbl.IsVisible = value;
                        TitleLbl.FadeTo(1, 500);
                    }
                    else if (string.IsNullOrEmpty(EntryCtr.Text))
                    {
                        TitleLbl.FadeTo(0, 500);
                        TitleLbl.IsVisible = value;
                    }
                    _isFocused = value;
                }
            }
        }

        public static readonly BindableProperty EntryAutomationIDProperty = BindableProperty.Create(
            propertyName: nameof(EntryAutomationID),
            returnType: typeof(string),
            declaringType: typeof(InputControl),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay);

        public string EntryAutomationID
        {
            get { return (string)GetValue(EntryAutomationIDProperty); }
            set { SetValue(EntryAutomationIDProperty, value); }
        }

        public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
            propertyName: nameof(Placeholder),
            returnType: typeof(string),
            declaringType: typeof(InputControl),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }


        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(InputControl),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is InputControl ctrl && ctrl.IsVisibleTopLabel && !ctrl.IsFocused)
                {
                    ctrl.TitleLbl.IsVisible = !string.IsNullOrEmpty(newValue as string);
                }
            });

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            propertyName: nameof(IsPassword),
            returnType: typeof(bool),
            declaringType: typeof(InputControl),
            defaultValue: default(bool),
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public static readonly BindableProperty EntryStyleProperty = BindableProperty.Create(
            propertyName: nameof(EntryStyle),
            returnType: typeof(Style),
            declaringType: typeof(InputControl),
            defaultValue: default(Style),
            defaultBindingMode: BindingMode.TwoWay);

        public string EntryStyle
        {
            get { return (string)GetValue(EntryStyleProperty); }
            set { SetValue(EntryStyleProperty, value); }
        }

        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
            propertyName: nameof(LabelStyle),
            returnType: typeof(Style),
            declaringType: typeof(InputControl),
            defaultValue: default(Style),
            defaultBindingMode: BindingMode.TwoWay);

        public string LabelStyle
        {
            get { return (string)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public static readonly BindableProperty IsVisibleTopLabelProperty = BindableProperty.Create(
            propertyName: nameof(IsVisibleTopLabel),
            returnType: typeof(bool),
            declaringType: typeof(InputControl),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsVisibleTopLabel
        {
            get { return (bool)GetValue(IsVisibleTopLabelProperty); }
            set { SetValue(IsVisibleTopLabelProperty, value); }
        }

        public static readonly BindableProperty IsEnabledEntryProperty = BindableProperty.Create(
           propertyName: nameof(IsEnabledEntry),
           returnType: typeof(bool),
           declaringType: typeof(InputControl),
           defaultValue: true,
           defaultBindingMode: BindingMode.TwoWay);

        public bool IsEnabledEntry
        {
            get { return (bool)GetValue(IsEnabledEntryProperty); }
            set { SetValue(IsEnabledEntryProperty, value); }
        }

        public static readonly BindableProperty DescriptionLine1Property = BindableProperty.Create(
            propertyName: nameof(DescriptionLine1),
            returnType: typeof(string),
            declaringType: typeof(InputControl),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay);

        public string DescriptionLine1
        {
            get { return (string)GetValue(DescriptionLine1Property); }
            set { SetValue(DescriptionLine1Property, value); }
        }

        public static readonly BindableProperty DescriptionLine2Property = BindableProperty.Create(
            propertyName: nameof(DescriptionLine2),
            returnType: typeof(string),
            declaringType: typeof(InputControl),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay);

        public string DescriptionLine2
        {
            get { return (string)GetValue(DescriptionLine2Property); }
            set { SetValue(DescriptionLine2Property, value); }
        }

        public static readonly BindableProperty KeyboardTypeProperty = BindableProperty.Create(
            propertyName: nameof(KeyboardType),
            returnType: typeof(Keyboard),
            declaringType: typeof(InputControl),
            defaultValue: default(Keyboard),
            defaultBindingMode: BindingMode.TwoWay);

        public Keyboard KeyboardType
        {
            get { return (Keyboard)GetValue(KeyboardTypeProperty); }
            set { SetValue(KeyboardTypeProperty, value); }
        }

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            propertyName: nameof(MaxLength),
            returnType: typeof(int),
            declaringType: typeof(InputControl),
            defaultValue: 1000,
            defaultBindingMode: BindingMode.TwoWay);

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
            propertyName: nameof(ErrorText),
            returnType: typeof(string),
            declaringType: typeof(InputControl),
            defaultValue: default(string),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (!string.IsNullOrEmpty(newValue as string))
                {
                    if (bindable is InputControl ctrl)
                    {
                        ctrl.EntryCtr.BorderColor = ctrl.ErrorLbl.TextColor;
                        ctrl.EntryCtr.TextColor = ctrl.ErrorLbl.TextColor;
                    }
                }
            });

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set
            {
                SetValue(ErrorTextProperty, value);
            }
        }

        public static readonly BindableProperty BehaviorsEntryProperty = BindableProperty.Create(
            propertyName: nameof(BehaviorsEntry),
            returnType: typeof(IList<Behavior>),
            declaringType: typeof(InputControl),
            defaultValue: default(IList<Behavior>),
            defaultBindingMode: BindingMode.TwoWay);

        public IList<Behavior> BehaviorsEntry
        {
            get { return (IList<Behavior>)GetValue(BehaviorsEntryProperty); }
            set { SetValue(BehaviorsEntryProperty, value); }
        }
    }
}
