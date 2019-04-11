using System;
using System.Reflection;
using System.Threading.Tasks;
using CuttingSystem3mkMobile.Entities;
using CuttingSystem3mkMobile.Entities.Response;

namespace CuttingSystem3mkMobile.RestAPI
{
    public class MockRemoteService : BaseRestClient, IRemoteService
    {
        private byte[] FakeDocument(string name)
        {
            Assembly currentAssembly = typeof(MockRemoteService).GetTypeInfo().Assembly;
            byte[] buffer = null;
            using (var resourceStream = currentAssembly.GetManifestResourceStream($"CuttingSystem3mkMobile.{name}"))
            {
                buffer = new byte[resourceStream.Length];
                resourceStream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }

        public Task<ServiceStatusMessage<DevicesResponse>> LoadDevices()
        {
            var response = new DevicesResponse()
            {
                Devices = new DeviceDetails[]
                {
                    new DeviceDetails()
                    {
                         Id = 1,
                         Name = "asus-zenfone-2-delux"
                    },
                    new DeviceDetails()
                    {
                         Id = 2,
                         Name = "asus-zenfone-2-delux"
                    },
                    new DeviceDetails()
                    {
                         Id = 3,
                         Name = "asus-zenfone-2-delux"
                    },
                    new DeviceDetails()
                    {
                         Id = 4,
                         Name = "asus-zenfone-2-delux"
                    },
                    new DeviceDetails()
                    {
                         Id = 5,
                         Name = "asus-zenfone-2-delux"
                    },
                    new DeviceDetails()
                    {
                         Id = 6,
                         Name = "asus-zenfone-2-delux"
                    }
                }
            };
            var serviceStatus = new ServiceStatusMessage<DevicesResponse>()
            {
                DidSucceed = true,
                Entity = response
            };

            var tcs = new TaskCompletionSource<ServiceStatusMessage<DevicesResponse>>();
            tcs.SetResult(serviceStatus);
            return tcs.Task;
        }

        public Task<ServiceStatusMessage<ModelsResponse>> LoadModels(int deviceId, string token)
        {
            byte[] fileData = FakeDocument("asus-zenfone-2-delux.plt");
            byte[] imageData = FakeDocument("foilIcon.png");

            var response = new ModelsResponse()
            {
                Models = new ModelDetails[]
                 {
                     new ModelDetails()
                     {
                          Id = 1,
                          Name = "asus-zenfone-2-delux",
                          FileData = fileData,
                          ImageData = imageData,
                          Side = Enums.ModelSideEnum.Front
                     },
                     new ModelDetails()
                     {
                          Id = 2,
                          Name = "asus-zenfone-2-delux",
                          FileData = fileData,
                          ImageData = imageData,
                          Side = Enums.ModelSideEnum.Front
                     },
                     new ModelDetails()
                     {
                          Id = 3,
                          Name = "asus-zenfone-2-delux",
                          FileData = fileData,
                          ImageData = imageData,
                          Side = Enums.ModelSideEnum.Front
                     }
                 }
            };
            var serviceStatus = new ServiceStatusMessage<ModelsResponse>()
            {
                DidSucceed = true,
                Entity = response
            };

            var tcs = new TaskCompletionSource<ServiceStatusMessage<ModelsResponse>>();
            tcs.SetResult(serviceStatus);
            return tcs.Task;
        }

        Task<ServiceStatusMessage<bool>> IRemoteService.ValidateCutCode(string code)
        {
            var serviceStatus = new ServiceStatusMessage<bool>()
            {
                Entity = true,
                DidSucceed = true
            };

            var tcs = new TaskCompletionSource<ServiceStatusMessage<bool>>();
            tcs.SetResult(serviceStatus);
            return tcs.Task;
        }

        public Task<ServiceStatusMessage<bool>> SetDisabledCode(string code)
        {
            var serviceStatus =  new ServiceStatusMessage<bool>()
            {
                Entity = true,
                DidSucceed = true
            };

            var tcs = new TaskCompletionSource<ServiceStatusMessage<bool>>();
            tcs.SetResult(serviceStatus);
            return tcs.Task;
        }
    }
}
