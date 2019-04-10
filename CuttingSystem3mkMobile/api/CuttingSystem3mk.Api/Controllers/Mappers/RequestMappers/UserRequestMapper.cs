using AutoMapper;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.WebApi.Model.Requests;

namespace CuttingSystem3mkMobile.WebApi.Controllers.Mappers
{
    public class UserRequestMapper : Profile
    {
        public UserRequestMapper()
        {
            CreateMap<UserRequest, User>();
        }
    }
}
