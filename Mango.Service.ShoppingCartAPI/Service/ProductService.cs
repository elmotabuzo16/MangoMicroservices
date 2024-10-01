using Mango.Service.ShoppingCartAPI.Models.Dtos;
using Mango.Service.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Service.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            // Product because the AddHttpClient key is Product from Program.cs
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");

            var apiContent = await response.Content.ReadAsStringAsync();
            var responseConvert = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (responseConvert.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseConvert.Result));
            }

            return new List<ProductDto>();
        }
    }
}
