using AutoMapper;
using Mango.Services.OrderAPI.Dtos;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.OrderDto;

namespace Mango.Services.OrderAPI.MappingProfile
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // Mapping between OrderHeaderDto and CartHeaderDto
            CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal))
                .ReverseMap();

            // Mapping between CartDetailsDto and OrderDetailsDto
            CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, u => u.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty)) // Handle null Product
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product != null ? src.Product.Price : 0.0)); // Handle null Product

            // Reverse mapping for OrderDetailsDto -> CartDetailsDto
            CreateMap<OrderDetailsDto, CartDetailsDto>();

            // Mapping between OrderHeader and OrderHeaderDto
            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();

            // Mapping between OrderDetails and OrderDetailsDto
            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
        }
    }
}
