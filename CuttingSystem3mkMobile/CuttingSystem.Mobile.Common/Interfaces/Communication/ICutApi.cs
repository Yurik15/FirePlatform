using System.Collections.Generic;
using Refit;
using System.Threading.Tasks;
using CuttingSystem.Mobile.Common.Entities.Container;
using CuttingSystem.Mobile.Common.Entities;

namespace CuttingSystem.Mobile.Common.Interfaces.Communication
{
    public interface ICutApi
    {
        [Get("/api/Cut/Devices")]
        Task<ApiContainer<DeviceModel>> GetDeviceModels();

        [Get("/api/Cut/Models")]
        Task<List<CutModel>> GetCutModelsByDevice(int idDevice);
    }
}
