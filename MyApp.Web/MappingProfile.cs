using AutoMapper;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Models.User;
using MyApp.Web.Models;

namespace MyApp.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserGetDto, User>();
            CreateMap<User, UserGetDto>();
            CreateMap<UserCreateUpdateDto, User>();
            CreateMap<User, UserCreateUpdateDto>();
        }
    }
}
