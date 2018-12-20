using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using FirePlatform.Mobile.Models;
using FirePlatform.Mobile.Tools;
using Xamarin.Forms;

namespace FirePlatform.Mobile.PageModels
{
    public class SettingsPageModel : BasePageModel, INotifyPropertyChanged
    {
        #region fields
        private LanguageResource _selectedItem;
        #endregion fields

        #region props

        public List<LanguageResource> ListItems { get; set; }
        public LanguageResource SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                SetCulture();
            }
        }
        public string VersionText { get; set; }
        #endregion

        #region ctor

        public SettingsPageModel()
        {
            ListItems = new List<LanguageResource>()
            {
                new LanguageResource(){ Id = 0, Name = "English", Src = "uk_flag.png", Cuture = "en"},
                new LanguageResource(){ Id = 1, Name = "Polski", Src = "pl_flag.png", Cuture = "pl"}
            };
        }

        #endregion

        #region Command Logic

        private ICommand _LogOutClickCommand;

        public ICommand LogOutClickCommand
        {
            get
            {
                if (_LogOutClickCommand == null)
                {
                    _LogOutClickCommand = new Command(() => this.LogOutButtonClick());
                }
                return _LogOutClickCommand;
            }
        }

        private void LogOutButtonClick()
        {
           
        }

        #endregion

        #region FreshMVVM override

        public override void Init(object initData)
        {
            SelectedItem = ListItems.FirstOrDefault(x => x.Cuture == Settings.Culture);
            base.Init(initData);
        }

        #endregion

        private void SetCulture()
        {
            Settings.Culture = SelectedItem.Cuture;
            AppResources.Culture = new System.Globalization.CultureInfo(SelectedItem.Cuture);
            NotifyPropertiesChanged();
        }
    }
}
