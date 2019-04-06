using AutoMapper;
using CuttingSystem3mk.Models.Models;
using CuttingSystem3mk.WebApi.Model.Requests;

namespace CuttingSystem3mk.WebApi.Controllers.Mappers
{
    public class UserRequestMapper : Profile
    {
        public UserRequestMapper()
        {
            CreateMap<UserRequest, User>();
        }
    }
}
