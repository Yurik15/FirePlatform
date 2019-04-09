using System;
using System.Threading.Tasks;
using CuttingSystem3mkMobile.Entities.Response;

namespace CuttingSystem3mkMobile.RestAPI
{
    public interface IRemoteService
    {
        Task<ServiceStatusMessage<DevicesResponse>> LoadDevices(int customerId);
        Task<ServiceStatusMessage<ModelsResponse>> LoadModels(int customerId, int deviceId);
    }
}
