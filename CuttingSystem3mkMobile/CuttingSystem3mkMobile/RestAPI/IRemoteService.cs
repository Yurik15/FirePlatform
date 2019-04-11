using System;
using System.Threading.Tasks;
using CuttingSystem3mkMobile.Entities.Response;

namespace CuttingSystem3mkMobile.RestAPI
{
    public interface IRemoteService
    {
        Task<ServiceStatusMessage<DevicesResponse>> LoadDevices();
        Task<ServiceStatusMessage<ModelsResponse>> LoadModels(int idDevice, string token);
        Task<ServiceStatusMessage<bool>> ValidateCutCode(string code);
        Task<ServiceStatusMessage<bool>> SetDisabledCode(string code);
    }
}
