using AutoMapper;
using FirePlatform.Models.Containers;
using FirePlatform.Services;
using FirePlatform.WebApi.Model.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FirePlatform.Models.Models;

namespace FirePlatform.WebApi.Controllers
{
    [ApiController]
    public class OrdersController : BaseController
    {
        public OrdersController
            (
                Service service,
                IMapper mapper
            )
            : base(service, mapper) { }


        [HttpGet("api/[controller]/Forms")]
        [ProducesResponseType(200, Type = typeof(FormResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiContainer<FormResponse>>> GetAll()
        {
            var result = await Service.GetFormService().GetAllForms();
            if (result == null)
                return NotFound();

            var response = Mapper.Map<IEnumerable<Form>, IEnumerable<FormResponse>>(result.DataCollection);
            var container = new ApiContainer<FormResponse>
            {
                DataCollection = response
            };

            return Ok(container);
        }
    }
}
