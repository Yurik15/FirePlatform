using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using CuttingSystem3mkMobile.Controls;
using CuttingSystem3mkMobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace CuttingSystem3mkMobile.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null && Element is CustomEntry view && view.IsVisibleBottomLine)
            {
                var color = view.BorderColor.ToAndroid();
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(color);
                else
                    Control.Background.SetColorFilter(color, PorterDuff.Mode.SrcAtop);
            }
        }
        bool _isDisposed;
        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            base.Dispose(disposing);
            GC.SuppressFinalize(this);
        }

    }
}
