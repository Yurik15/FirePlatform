using AutoMapper;
using CuttingSystem3mk.Models.Models;
using CuttingSystem3mk.WebApi.Model.Responses;

namespace CuttingSystem3mk.WebApi.Controllers.Mappers.ResponseMappers
{
    public class UserResponseMapper : Profile
    {
        public UserResponseMapper()
        {
            CreateMap<User, UserResponse>();
        }
    }
}
