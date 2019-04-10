using CuttingSystem3mkMobile.Models.Containers;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Repositories;
using CuttingSystem3mkMobile.Repositories.Repositories;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Services.Services
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
