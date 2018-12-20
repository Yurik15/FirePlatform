using System;
using System.Collections.Generic;
using FirePlatform.Mobile.PageModels;
using FirePlatform.Mobile.Tools.SyncfusionTools;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using System.Linq;
using ItemControl = FirePlatform.Mobile.Common.Entities.Item;

namespace FirePlatform.Mobile.Pages
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            listView.ChildAdded += ListView_ChildAdded;
            listView.GroupCollapsed += ListView_GroupCollapsed;

            var settingsToolbarItem = new ToolbarItem();
            settingsToolbarItem.Icon = "settingsIcon.png";
            settingsToolbarItem.Clicked += SettingsToolbarItem_Clicked;
            ToolbarItems.Add(settingsToolbarItem);
        }

        void SettingsToolbarItem_Clicked(object sender, EventArgs e)
        {
        }


        void ListView_GroupCollapsed(object sender, GroupExpandCollapseChangedEventArgs e)
        {

        }


        void ListView_ChildAdded(object sender, ElementEventArgs e)
        {
            var viewmodel = (BindingContext as HomePageModel);
            if (viewmodel != null)
            {
                viewmodel.ItemsControlLoader.CollapseExpandGroupAction = (nameGroup, expanded) =>
                {
                    if (listView.DataSource.Groups.Count > 0)
                    {
                        var foundGroup = listView.DataSource.Groups.FirstOrDefault(x => x.Key.ToString() == nameGroup);
                        if (foundGroup != null)
                        {
                            if (!foundGroup.IsExpand && expanded)
                            {
                                listView.ExpandGroup(foundGroup);
                            }
                            else if (foundGroup.IsExpand && !expanded)
                            {
                                listView.CollapseGroup(foundGroup);
                            }
                        }
                    }
                };
                viewmodel.ItemsControlLoader.CollapseExpand();
            }
        }

    }
}
