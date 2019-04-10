using CuttingSystem3mkMobile.Models.Containers;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Repositories;
using CuttingSystem3mkMobile.Repositories.Repositories;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Services.Services
{
    public class CutModelService : BaseService<CutModelService, CutModelRepository, CutModel>
    {
        public CutModelService
            (
                BaseRepository<CutModel, CutModelRepository> baseRepository,
                Repository repository
            ) : base(baseRepository, repository)
        {
            
        }

        public async Task<ServiceContainer<CutModel>> GetByDeviceId(int idDevice)
        {
            var cutModels = await Repository.CutModelRepository().Get(x => x.IdDeviceModel == idDevice);

            return new ServiceContainer<CutModel>()
            {
                DataCollection = cutModels
            };     
        }
    }
}
