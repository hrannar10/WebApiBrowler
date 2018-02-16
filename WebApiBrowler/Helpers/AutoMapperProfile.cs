using AutoMapper;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;

namespace WebApiBrowler.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Requests.RegistrationDto, AppUser>().ForMember(au => au.UserName, map => map.MapFrom(vm => vm.Email));

            CreateMap<Company, Responses.CompanyDto>();
            CreateMap<Requests.CompanyDto, Company>();

            CreateMap<Asset, Requests.AssetTypeDto>();
            CreateMap<Requests.AssetDto, Asset>();

            CreateMap<AssetType, Responses.AssetTypeDto>();
            CreateMap<Requests.AssetTypeDto, AssetType>();
        }
    }
}
