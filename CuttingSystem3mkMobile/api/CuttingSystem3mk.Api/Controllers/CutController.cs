using AutoMapper;
using CuttingSystem3mkMobile.Api.Controllers.Mappers.ResponseMappers;
using CuttingSystem3mkMobile.Api.Model.Responses;
using CuttingSystem3mkMobile.Models.Containers;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Services;
using CuttingSystem3mkMobile.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.Api.Controllers
{
    [ApiController]
    public class CutController : BaseController
    {
        public CutController
         (
             Service service,
             IMapper mapper
         )
         : base(service, mapper) { }

        [HttpGet("api/[controller]/Devices")]
        [ProducesResponseType(200, Type = typeof(DeviceModelResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiContainer<DeviceModelResponse>>> GetDeviceModels()
        {
            var result = await Service.GetDeviceModelService().Get();
            if (result == null)
                return NotFound();

            var response = Mapper.Map<IEnumerable<DeviceModel>, IEnumerable<DeviceModelResponse>>(result);
            var container = new ApiContainer<DeviceModelResponse>
            {
                DataCollection = response
            };

            return Ok(container);
        }

        [HttpGet("api/[controller]/Models/{idDevice}")]
        [ProducesResponseType(200, Type = typeof(CutModelResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiContainer<CutModelResponse>>> GetCutModels(int idDevice)
        {
            var result = (await Service.GetCutModelService().GetByDeviceId(idDevice)).DataCollection;
            if (!result.Any())
                return NotFound();        

            var response = Mapper.Map<IEnumerable<CutModel>, IEnumerable<CutModelResponse>>(result);
            var container = new ApiContainer<CutModelResponse>
            {
                DataCollection = response
            };

            return Ok(container);
        }
    }
}
