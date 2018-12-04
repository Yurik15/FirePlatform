using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirePlatform.Mobile.Common;
using FirePlatform.Mobile.Common.Entities;
using FirePlatform.Mobile.Common.Implements;
using FirePlatform.Mobile.Common.Tools;
using Xamarin.Forms;

namespace FirePlatform.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var url = $"https://onedrive.live.com/download.aspx?cid=15B0AEBC04DB9C80&authKey=%21AODJTEr%5FsKI%2D5Hs&resid=15B0AEBC04DB9C80%21145&canary=Ob4rLa3XhEEl%2BLpIaWfPs40HZ6chYRmnfdoXnngg%2FBQ%3D2&ithint=%2Exml";
            var parserXml = new ParserXml<ArrayOfItemGroupSer>();
            var a = parserXml.Deserialize(url);
        }
    }
}
