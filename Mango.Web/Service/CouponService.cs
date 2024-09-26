using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.CouponAPIBaseURL + "/api/coupon",
            });

            return result;
        }

        public async Task<ResponseDto> GetCouponByCodeAsync(string couponCode)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.CouponAPIBaseURL + "/api/coupon/GetByCode/" + couponCode,
            });

            return result;
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int Id)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.CouponAPIBaseURL + "/api/coupon/" + Id,
            });

            return result;
        }

        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.CouponAPIBaseURL + "/api/coupon",
                Data = couponDto
            });

            return result;
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                ApiUrl = StaticDetails.CouponAPIBaseURL + "/api/coupon",
                Data = couponDto
            });

            return result;
        }

        public async Task<ResponseDto> DeleteCouponAsync(int Id)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                ApiUrl = StaticDetails.CouponAPIBaseURL + "/api/coupon/" + Id,
            });

            return result;
        }

    }
}
