using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.OrderAPIBaseURL + "/api/order/CreateOrder",
                Data = cartDto
            });

            return result;
        }
    }
}
