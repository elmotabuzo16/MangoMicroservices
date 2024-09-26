using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.MappingProfile
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<CouponDto, Coupon>().ReverseMap();
        }
    }
}
