using System.Text.Json;
using AutoMapper;
using Rangle.Abstractions.Entities;
using Rangle.API.Models;

namespace Rangle.API.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Entity, EntityModel>().ForMember(e => e.JsonObject, opt => opt.MapFrom(e => JsonSerializer.Deserialize<object>(e.JsonObject, null))).ReverseMap();

            CreateMap<UserEntity, UserModel>().ReverseMap();
            CreateMap<UserSignInModel, UserEntity>().ReverseMap();
            CreateMap<UserRegisterModel, UserEntity>().ReverseMap();
        }
    }
}
