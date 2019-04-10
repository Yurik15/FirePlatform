using AutoMapper;
using CuttingSystem3mkMobile.Models.Models;
using CuttingSystem3mkMobile.WebApi.Model.Responses;

namespace CuttingSystem3mkMobile.WebApi.Controllers.Mappers.ResponseMappers
{
    public class UserResponseMapper : Profile
    {
        public UserResponseMapper()
        {
            CreateMap<User, UserResponse>();
        }
    }
}
