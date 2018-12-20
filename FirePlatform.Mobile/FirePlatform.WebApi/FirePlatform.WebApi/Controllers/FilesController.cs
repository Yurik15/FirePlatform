using AutoMapper;
using FirePlatform.Services;
using Microsoft.AspNetCore.Mvc;
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
    }
}
