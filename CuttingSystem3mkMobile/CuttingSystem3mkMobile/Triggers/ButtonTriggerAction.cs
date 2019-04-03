using System;
using Xamarin.Forms;

namespace CuttingSystem3mkMobile.Triggers
{
    public class ButtonTriggerAction : TriggerAction<VisualElement>
    {
        public Color BorderColor { get; set; }
        public Color TextColor { get; set; }
        public Color BackgroundColor { get; set; }

        protected override void Invoke(VisualElement sender)
        {
            if (sender is Button button)
            {
                if (BorderColor != default(Color)) button.BorderColor = BorderColor;
                if (TextColor != default(Color)) button.TextColor = TextColor;
                if (BackgroundColor != default(Color)) button.BackgroundColor = BackgroundColor;
            }
        }
    }
}
