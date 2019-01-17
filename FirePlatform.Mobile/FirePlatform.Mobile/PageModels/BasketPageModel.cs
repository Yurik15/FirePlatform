using FirePlatform.Mobile.Common.Interfaces.Communication;
using FirePlatform.Mobile.Common.Models;
using Refit;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FirePlatform.Mobile.PageModels
{
    public class BasketPageModel : BasePageModel
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

        public BasketPageModel()
        {
            RefreshData();
        }

        #endregion


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

            }

            #endregion
        }
    }
}
