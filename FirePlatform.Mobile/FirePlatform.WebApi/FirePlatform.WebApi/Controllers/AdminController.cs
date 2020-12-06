using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirePlatform.Models.Models;
using FirePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirePlatform.WebApi.Controllers
{
    [ApiController]
    public class AdminController : BaseController
    {
        public AdminController
            (
                Service service,
                IMapper mapper
            ) : base(service, mapper)
        {
        }


        [HttpGet("api/[controller]/LoadTemplates")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [AllowAnonymous]
        [EnableCors("AllowAll")]
        //[Authorize]
        public async Task<OkObjectResult> LoadTemplates()
        {
            var scriptDefinitionService = Service.GetScriptDefinitionService();
            var items = await scriptDefinitionService.GetAsync();
            if (items == null || !items.Any())
            {
                items = scriptDefinitionService.LoadRemoteScripts();
                if (items != null)
                {
                    await scriptDefinitionService.SaveAll(items);
                }
            }
            return Ok(items);
        }
    }
}