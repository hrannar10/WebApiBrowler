using AutoMapper;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;

namespace WebApiBrowler.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationDto, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));
        }
    }
}
