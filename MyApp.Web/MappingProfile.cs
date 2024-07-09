using AutoMapper;
using MyApp.Infrastructure.Data;
using MyApp.Web.Models;

namespace MyApp.Web
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User > ();
        }
    }
}
