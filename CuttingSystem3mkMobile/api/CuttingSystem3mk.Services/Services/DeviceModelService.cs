using CuttingSystem3mk.Models.Containers;
using CuttingSystem3mk.Models.Models;
using CuttingSystem3mk.Repositories;
using CuttingSystem3mk.Repositories.Repositories;
using System.Threading.Tasks;

namespace CuttingSystem3mk.Services.Services
{
    public class DeviceModelService : BaseService<DeviceModelService, DeviceModelRepository, DeviceModel>
    {
        public DeviceModelService
            (
                BaseRepository<DeviceModel, DeviceModelRepository> baseRepository,
                Repository repository
            ) : base(baseRepository, repository)
        {

        }
    }
}
