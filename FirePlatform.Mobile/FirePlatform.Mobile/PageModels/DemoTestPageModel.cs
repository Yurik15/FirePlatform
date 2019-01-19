using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FirePlatform.Mobile.Common.Entities;
using FirePlatform.Mobile.Common.Interfaces.Communication;
using FirePlatform.Mobile.Common.Models;
using Refit;

namespace FirePlatform.Mobile.PageModels
{
    public class DemoTestPageModel : BasePageModel
    {
        public List<FormTreeResponse> Data { get; set; }
        public DemoTestPageModel()
        {
            Data = new List<FormTreeResponse>();
        }
        public async void LoadData(int id, string value, int dataCount = 10)
        {
            IsBusy = true;
            await Task.Run(async () =>
            {
                var apiResponse = RestService.For<ITemplateModelApi>(RestApiServerUri);
                Data = await apiResponse.DemoTest(id, value, dataCount);
                IsBusy = false;
            });
        }
    }
}
