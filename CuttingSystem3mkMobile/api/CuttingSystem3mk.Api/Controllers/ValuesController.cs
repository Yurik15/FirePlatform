using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CuttingSystem3mkMobile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "CutSystem API - @made by HackDream"};
        }
    }
}
