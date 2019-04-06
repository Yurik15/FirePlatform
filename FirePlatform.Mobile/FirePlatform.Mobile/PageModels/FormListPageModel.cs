using FirePlatform.Mobile.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Refit;
using FirePlatform.Mobile.Common.Interfaces.Communication;

namespace FirePlatform.Mobile.PageModels
{
    public class FormListPageModel : BasePageModel
    {
        #region fields

        private TemplateModel _selectedForm;

        #endregion

        #region prop
        private ObservableCollection<TemplateModel> _ReportedForms;
        public ObservableCollection<TemplateModel> ReportedForms
        {
            get => _ReportedForms;
            set
            {
                _ReportedForms = value;
                RaisePropertyChanged(nameof(ReportedForms));
            }
        }
        public TemplateModel SelectedForm
        {
            get { return _selectedForm; }
            set
            {
                _selectedForm = value;
                CoreMethods.PushPageModel<HomePageModel>();
            }
        }

        #endregion

        #region ctor

        public FormListPageModel()
        {
            RefreshData();
        }

        #endregion

        //#region FreshMVVM override

        //public override void Init(object initData)
        //{
        //    CurrentDate = (DateTime)initData;
        //    base.Init(initData);
        //}

        //#endregion

        //public override void LoadData()
        //{
        //    RefreshData();
        //}

        #region Navigation

        private void RefreshData()
        {
            try
            {
                IsBusy = true;
                Task.Run(async () =>
                 {
                     var apiResponse = RestService.For<ITemplateModelApi>(RestApiServerUri);
                     var container = await apiResponse.GetTemplateModels();
                     Device.BeginInvokeOnMainThread(() =>
                    {
                        ReportedForms = new ObservableCollection<TemplateModel>(container?.DataCollection);
                    });
                 });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion
        }
    }
}



