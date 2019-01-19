using FirePlatform.Mobile.Common.Entities;
using FirePlatform.Mobile.Common.Models;
using FirePlatform.Mobile.Common.Models.Container;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirePlatform.Mobile.Common.Interfaces.Communication
{
    public interface ITemplateModelApi
    {

        [Get("/api/Orders/Forms")]
        Task<ApiContainer<TemplateModel>> GetTemplateModels();

        [Get("/api/Files/Test")]
        Task<List<FormTreeResponse>> DemoTest(int id, string value, int dataCount = 10);
    }
}
