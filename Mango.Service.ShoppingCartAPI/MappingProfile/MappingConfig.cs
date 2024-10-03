using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;

namespace Mango.Services.ShoppingCartAPI.MappingProfile
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        }
    }
}
