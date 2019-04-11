using AutoMapper;
using CuttingSystem3mkMobile.Services;
using Microsoft.AspNetCore.Mvc;

namespace CuttingSystem3mkMobile.WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController
            (
                Service service,
                IMapper mapper
            )
        {
            Service = service;
            Mapper = mapper;
        }
        protected Service Service;
        protected IMapper Mapper;
    }
}
