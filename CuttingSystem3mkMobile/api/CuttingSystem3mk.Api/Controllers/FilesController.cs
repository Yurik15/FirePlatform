using AutoMapper;
using CuttingSystem3mkMobile.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CuttingSystem3mkMobile.WebApi.Controllers
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

        [HttpGet("api/[controller]/SynchronizeFiles")]
        public async Task SynchronizeFiles()
        {
            var fileList = new Dictionary<string, byte[]>();
            foreach(string file in Directory.GetFiles(@"C:\Users\Yurii\source\FirePlatform\CuttingSystem3mkMobile\api\CuttingSystem3mk.Api\wwwroot\Cut"))
            {
                var filepath = Path.Combine("~/Cut", file);
                var bytes = System.IO.File.ReadAllBytes(filepath);
                var fileName = file.Split("Cut\\")[1];
                var fileNameWithoutFormat = fileName.Substring(0, fileName.Length - 4);
                fileList.Add(fileNameWithoutFormat, bytes);
            }

            await Service.GetCutModelService().SynchronizeAddFilesToDb(fileList);
        }
        [HttpGet("api/[controller]/SynchronizeFileNames")]
        public async Task SynchronizeFileNames()
        {
            var fileList = new Dictionary<string, byte[]>();
            foreach (string file in Directory.GetFiles(@"C:\Users\Yurii\source\FirePlatform\CuttingSystem3mkMobile\api\CuttingSystem3mk.Api\wwwroot\CutNames"))
            {
                var filepath = Path.Combine("~/CutNames", file);
                var bytes = System.IO.File.ReadAllBytes(filepath);
                var fileName = file.Split("CutNames\\")[1];
                var fileNameWithoutFormat = fileName.Substring(0, fileName.Length - 4);
                fileList.Add(fileNameWithoutFormat, bytes);
            }

            await Service.GetCutModelService().SynchronizeFileNames(fileList);
        }
    }
}
