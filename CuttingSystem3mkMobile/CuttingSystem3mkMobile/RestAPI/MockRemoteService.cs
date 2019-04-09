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

        public Task<ServiceStatusMessage<DevicesResponse>> LoadDevices(int customerId)
        {
            byte[] deviceCover = FakeDocument("deviceIcon.png");
            var response = new DevicesResponse()
            {
                Devices = new DeviceDetails[]
                {
                    new DeviceDetails()
                    {
                         Id = Guid.NewGuid(),
                         Name = "asus-zenfone-2-delux",
                         ImageData = null
                    },
                    new DeviceDetails()
                    {
                         Id = Guid.NewGuid(),
                         Name = "asus-zenfone-2-delux",
                         ImageData = null
                    },
                    new DeviceDetails()
                    {
                         Id = Guid.NewGuid(),
                         Name = "asus-zenfone-2-delux",
                         ImageData = null
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

        public Task<ServiceStatusMessage<ModelsResponse>> LoadModels(int customerId, int deviceId)
        {
            byte[] modelFoil = FakeDocument("foilIcon.png");
            byte[] fileData = FakeDocument("asus-zenfone-2-delux.plt");

            var response = new ModelsResponse()
            {
                Models = new ModelDetails[]
                 {
                     new ModelDetails()
                     {
                          Id = Guid.NewGuid(),
                          Name = "asus-zenfone-2-delux",
                          FileData = null,
                          ImageData = null,
                          Side = Enums.ModelSideEnum.Front
                     },
                     new ModelDetails()
                     {
                          Id = Guid.NewGuid(),
                          Name = "asus-zenfone-2-delux",
                          FileData = null,
                          ImageData = null,
                          Side = Enums.ModelSideEnum.Front
                     },
                     new ModelDetails()
                     {
                          Id = Guid.NewGuid(),
                          Name = "asus-zenfone-2-delux",
                          FileData = null,
                          ImageData = null,
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
    }
}
