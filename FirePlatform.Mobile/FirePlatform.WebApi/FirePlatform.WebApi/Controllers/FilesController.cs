using AutoMapper;
using FirePlatform.Services;
using FirePlatform.WebApi.Model.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;

namespace FirePlatform.WebApi.Controllers
{
    [ApiController]
    public class FilesController : BaseController
    {
        public FilesController
            (
                Service service,
                IMapper mapper
            )
            : base(service, mapper) { }

        [HttpGet("api/[controller]")]
        public VirtualFileResult GetVirtualFile()
        {
            var filepath = Path.Combine("~/Files", "test.xml");
            return File(filepath, "xml/application", "test.xml");
        }

        //TODO remove it after tests
        [HttpGet("api/[controller]/Test")]
        [ProducesResponseType(200, Type = typeof(FormTreeResponse))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [EnableCors("AllowAll")]
        public OkObjectResult GetForTest(int id, string value, int dataCount = 10)
        {
            var formTreeResponse = new FormTreeResponse()
            {
                Id = 12345,
                Value = value
            };
            var result = new List<FormTreeResponse>();

            for (int i = 0; i < dataCount; i++)
            {
                result.Add(formTreeResponse);
            }

            return Ok(result);
        }
    }
}
