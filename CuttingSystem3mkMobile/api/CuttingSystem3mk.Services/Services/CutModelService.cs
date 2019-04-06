using CuttingSystem3mk.Models.Containers;
using CuttingSystem3mk.Models.Models;
using CuttingSystem3mk.Repositories;
using CuttingSystem3mk.Repositories.Repositories;
using System.Threading.Tasks;

namespace CuttingSystem3mk.Services.Services
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
