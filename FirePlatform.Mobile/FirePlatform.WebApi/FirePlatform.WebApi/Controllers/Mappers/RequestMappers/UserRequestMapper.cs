using AutoMapper;
using FirePlatform.Models.Models;
using FirePlatform.WebApi.Model.Requests;

namespace FirePlatform.WebApi.Controllers.Mappers
{
    public class UserRequestMapper : Profile
    {
        public UserRequestMapper()
        {
            CreateMap<UserRequest, Users>();
        }
    }
}
