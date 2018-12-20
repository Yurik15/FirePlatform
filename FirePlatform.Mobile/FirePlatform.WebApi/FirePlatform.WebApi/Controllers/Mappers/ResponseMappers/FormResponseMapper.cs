using AutoMapper;
using FirePlatform.Models.Models;
using FirePlatform.WebApi.Model.Responses;

namespace FirePlatform.WebApi.Controllers.Mappers.ResponseMappers
{
    public class FormResponseMapper : Profile
    {
        public FormResponseMapper()
        {
            CreateMap<TemplateModel, FormResponse>();
        }
    }
}
