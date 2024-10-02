using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseURL + "/api/cart/ApplyCoupon",
                Data = cartDto
            });

            return result;
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseURL + "/api/cart/GetCart/" + userId,
            });

            return result;
        }

        public async Task<ResponseDto?> RemoveCartAsync(int cartDetailsId)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseURL + "/api/cart/RemoveCart/" + cartDetailsId,
            });

            return result;
        }

        public async Task<ResponseDto?> RemoveCouponAsync(CartDto cartDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseURL + "/api/cart/RemoveCoupon",
                Data = cartDto
            });

            return result;
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ShoppingCartAPIBaseURL + "/api/cart/CartUpsert",
                Data = cartDto
            });

            return result;
        }
    }
}
