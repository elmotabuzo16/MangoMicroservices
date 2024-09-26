using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetAllCouponsAsync();
        Task<ResponseDto> GetCouponByIdAsync(int Id);
        Task<ResponseDto> GetCouponByCodeAsync(string couponCode);
        Task<ResponseDto> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> DeleteCouponAsync(int Id);
    }
}
