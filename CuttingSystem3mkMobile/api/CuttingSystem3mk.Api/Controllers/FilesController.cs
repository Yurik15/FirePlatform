using AutoMapper;
using CuttingSystem3mk.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CuttingSystem3mk.WebApi.Controllers
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
