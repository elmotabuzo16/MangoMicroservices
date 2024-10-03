using Mango.Services.OrderAPI.Dtos;

namespace Mango.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
    }
}
