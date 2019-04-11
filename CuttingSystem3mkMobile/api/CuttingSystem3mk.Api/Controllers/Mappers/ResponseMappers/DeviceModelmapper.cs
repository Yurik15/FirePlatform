using AutoMapper;
using CuttingSystem3mkMobile.Api.Model;
using CuttingSystem3mkMobile.Models.Models;

namespace CuttingSystem3mkMobile.Api.Controllers.Mappers.ResponseMappers
{
    public class DeviceModelMapper : Profile
    {
        public DeviceModelMapper()
        {
            CreateMap<DeviceModel, ModelDetails>();
        }
    }
}
