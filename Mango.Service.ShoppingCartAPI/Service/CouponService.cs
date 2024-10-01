using Mango.Service.ShoppingCartAPI.Models.Dtos;
using Mango.Service.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Service.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            // Product because the AddHttpClient key is Product from Program.cs
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");

            var apiContent = await response.Content.ReadAsStringAsync();
            var responseConvert = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (responseConvert.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseConvert.Result));
            }

            return new CouponDto();

        }
    }
}
