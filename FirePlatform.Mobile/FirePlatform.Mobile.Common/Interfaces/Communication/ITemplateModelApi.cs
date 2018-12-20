using FirePlatform.Mobile.Common.Models;
using FirePlatform.Mobile.Common.Models.Container;
using Refit;
using System.Threading.Tasks;

namespace FirePlatform.Mobile.Common.Interfaces.Communication
{
    public interface ITemplateModelApi
    {

        [Get("/api/Orders/Forms")]
        Task<ApiContainer<TemplateModel>> GetTemplateModels();

    }
}
