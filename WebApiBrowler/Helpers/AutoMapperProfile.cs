using AutoMapper;
using WebApiBrowler.Dtos.Request;
using WebApiBrowler.Entities;

namespace WebApiBrowler.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationDtoRequest, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));

            CreateMap<Company, Dtos.Response.CompanyDtoResponse>();
            CreateMap<Dtos.Request.CompanyDtoRequest, Company>();

            CreateMap<Asset, AssetDtoRequest>();
            CreateMap<AssetDtoRequest, Asset>();

            CreateMap<AssetType, Dtos.Response.AssetTypeDtoResponse>();
            CreateMap<Dtos.Request.AssetTypeDtoRequest, AssetType>();
        }
    }
}
