using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.Controls
{
    public class CustomEntry : Entry
    {
        private bool _IsHiddenSomeText = false;
        public void SetTextWasHidden(bool value)
        {
            _IsHiddenSomeText = value;
        }
        public bool GetTextWasHidden()
        {
            return _IsHiddenSomeText;
        }


        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            propertyName: nameof(BorderColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.TwoWay);
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public bool IsVisibleBottomLine
        {
            get { return (bool)GetValue(IsVisibleBottomLineProperty); }
            set { SetValue(IsVisibleBottomLineProperty, value); }
        }

        public static readonly BindableProperty IsVisibleBottomLineProperty = BindableProperty.Create(
            propertyName: nameof(IsVisibleBottomLine),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);



        public static readonly BindableProperty InactiveColorProperty = BindableProperty.Create(
            propertyName: nameof(InactiveColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.TwoWay);

        public Color InactiveColor
        {
            get { return (Color)GetValue(InactiveColorProperty); }
            set { SetValue(InactiveColorProperty, value); }
        }

        public static readonly BindableProperty PressedColorProperty = BindableProperty.Create(
            propertyName: nameof(PressedColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.TwoWay);

        public Color PressedColor
        {
            get { return (Color)GetValue(PressedColorProperty); }
            set { SetValue(PressedColorProperty, value); }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == nameof(IsFocused))
            {
                if (_IsHiddenSomeText && IsFocused && !string.IsNullOrEmpty(Text))
                {
                    Text = string.Empty;
                }
                BorderColor = IsFocused || !string.IsNullOrEmpty(Text) ? PressedColor : InactiveColor;
                TextColor = BorderColor;
            }
            else if (propertyName == nameof(Text) && !IsFocused)
            {
                BorderColor = string.IsNullOrEmpty(Text) ? InactiveColor : PressedColor;
                TextColor = BorderColor;
            }
            base.OnPropertyChanged(propertyName);
        }
    }
}
