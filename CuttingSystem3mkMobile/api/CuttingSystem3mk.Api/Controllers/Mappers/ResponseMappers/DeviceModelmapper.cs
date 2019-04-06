using AutoMapper;
using CuttingSystem3mk.Api.Model.Responses;
using CuttingSystem3mk.Models.Models;

namespace CuttingSystem3mk.Api.Controllers.Mappers.ResponseMappers
{
    public class DeviceModelMapper : Profile
    {
        public DeviceModelMapper()
        {
            CreateMap<DeviceModel, DeviceModelResponse>();
        }
    }
}
