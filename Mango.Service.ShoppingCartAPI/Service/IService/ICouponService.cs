using Mango.Service.ShoppingCartAPI.Models.Dtos;

namespace Mango.Service.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
