using AutoMapper;
using CuttingSystem3mk.Api.Controllers.Mappers.ResponseMappers;
using CuttingSystem3mk.Api.Model;
using CuttingSystem3mkMobile.Api.Model;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.Services;
using CuttingSystem3mkMobile.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceDetails = CuttingSystem3mkMobile.Api.Model.DeviceDetails;

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
        [ProducesResponseType(200, Type = typeof(DeviceResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<DeviceResponse>> GetDeviceModels()
        {
            var result = await Service.GetDeviceModelService().Get();
            if (result == null)
                return NotFound();

            var dataCollection = Mapper.Map<IEnumerable<DeviceModel>, IEnumerable<DeviceDetails>>(result);
            var response = new DeviceResponse
            {
                Devices = dataCollection
            };

            return Ok(response);
        }

        [HttpGet("api/[controller]/Models/{idDevice}/{token}")]
        [ProducesResponseType(200, Type = typeof(ModelResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ModelResponse>> GetModels(int idDevice, string token)
        {
            if (!token.Equals("a8639ed3-3d5d-4482-86ab-fadec95728cc"))
                return NotFound();

            var result = (await Service.GetCutModelService().GetByDeviceId(idDevice)).DataCollection;
            if (!result.Any())
                return NotFound();

            var dataCollection = Mapper.Map<IEnumerable<CutModel>, IEnumerable<ModelDetails>>(result);
            var response = new ModelResponse
            {
                Models = dataCollection
            };

            return Ok(response);
                                            }

        [HttpGet("api/[controller]/ValidateCode/{code}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> ValidateCode(string code)
        {
            var isValidCode = await Service.GetCutCodeService().CheckCodes(code);
            if(isValidCode == null)
                return NotFound();


            return Ok(isValidCode);
        }

        [HttpGet("api/[controller]/GenerateCodes/{count}")]
        [ProducesResponseType(200, Type = typeof(CutCodeDetails))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<CutCodeDetails>>> GenerateCodes(int count)
        {
            var cutCodes = await Service.GetCutCodeService().GenerateCodes(count, 6);

            var dataCollection = Mapper.Map<IEnumerable<CutCode>, IEnumerable<CutCodeDetails>>(cutCodes);
            var response = new CutCodeResponse
            {
                CutCodes = dataCollection
            };
            return Ok(response);
        }
    }
}
