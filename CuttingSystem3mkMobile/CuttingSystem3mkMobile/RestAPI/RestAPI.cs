using System;
using System.Threading.Tasks;
using CuttingSystem3mkMobile.Entities.Response;

namespace CuttingSystem3mkMobile.RestAPI
{
    public class RestAPI : BaseRestClient, IRemoteService
    {
        #region Devices
        public async Task<ServiceStatusMessage<DevicesResponse>> LoadDevices()
        {
            return await MakeGetRequestReturnObject<DevicesResponse>("Cut/Devices", false);
        }
        #endregion

        #region Models
        public async Task<ServiceStatusMessage<ModelsResponse>> LoadModels(int idDevice, string token)
        {
            return await MakeGetRequestReturnObject<ModelsResponse>("Cut/Models/" + idDevice + "/" + token, false);
        }
        #endregion

        #region CutCodes
        public async Task<ServiceStatusMessage<bool>> SetDisabledCode(string code)
        {
            return await MakeGetRequestReturnBool("Cut/SetDisabledCode/" + code, false);
        }

        public async Task<ServiceStatusMessage<bool>> ValidateCutCode(string code)
        {
            return await MakeGetRequestReturnBool("Cut/ValidateCode/" + code, false);
        }
        #endregion

    }
}
