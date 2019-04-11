using AutoMapper;
using CuttingSystem3mkMobile.Api.Model;
using CuttingSystem3mkMobile.Models.Models;
using System;
using System.Text;

namespace CuttingSystem3mk.Api.Controllers.Mappers.ResponseMappers
{
    public class ModelResponseMapper : Profile
    {
        public ModelResponseMapper()
        {
            CreateMap<CutModel, ModelDetails>()
                 .ForMember(x => x.FileData, opt => opt.MapFrom(c => Convert.FromBase64String(c.CutFile.Value)))
                 .ForMember(x => x.ImageData, opt => opt.MapFrom(c => Convert.FromBase64String(c.CutFile.ImageValue)));
        }
    }
}
