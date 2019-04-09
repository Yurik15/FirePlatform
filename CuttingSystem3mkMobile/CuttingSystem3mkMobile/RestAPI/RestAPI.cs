using System;
using System.Threading.Tasks;
using CuttingSystem3mkMobile.Entities.Response;

namespace CuttingSystem3mkMobile.RestAPI
{
    public class RestAPI : BaseRestClient, IRemoteService
    {
        public Task<ServiceStatusMessage<DevicesResponse>> LoadDevices(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceStatusMessage<ModelsResponse>> LoadModels(int customerId, int deviceId)
        {
            throw new NotImplementedException();
        }
    }
}
