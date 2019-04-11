using System;
using System.Threading.Tasks;
using CuttingSystem3mkMobile.Entities.Response;

namespace CuttingSystem3mkMobile.RestAPI
{
    public class RestAPI : BaseRestClient, IRemoteService
    {
        public async Task<ServiceStatusMessage<DevicesResponse>> LoadDevices()
        {
            return await MakeGetRequestReturnObject<DevicesResponse>("Cut/Devices", false);
        }

        public async Task<ServiceStatusMessage<ModelsResponse>> LoadModels(int idDevice, string token)
        {
            return await MakeGetRequestReturnObject<ModelsResponse>("Cut/Models/" + idDevice + "/" + token, false);
        }

        public async Task<ServiceStatusMessage<bool>> ValidateCutCode(string code)
        {
            return await MakeGetRequestReturnBool("Cut/ValidateCode/" + code, false);
        }
    }
}
