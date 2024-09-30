using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> GetAllProductsAsync()
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.ProductAPIBaseURL + "/api/product",
            });

            return result;
        }

        public async Task<ResponseDto> GetProductByIdAsync(int Id)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                ApiUrl = StaticDetails.ProductAPIBaseURL + "/api/product/" + Id,
            });

            return result;
        }
        public async Task<ResponseDto> CreateProductAsync(ProductDto productDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.ProductAPIBaseURL + "/api/product",
                Data = productDto
            });

            return result;
        }


        public async Task<ResponseDto> UpdateProductAsync(ProductDto productDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                ApiUrl = StaticDetails.ProductAPIBaseURL + "/api/product",
                Data = productDto
            });

            return result;
        }

        public async Task<ResponseDto> DeleteProductAsync(int Id)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                ApiUrl = StaticDetails.ProductAPIBaseURL + "/api/product/" + Id,
            });

            return result;
        }
    }
}
