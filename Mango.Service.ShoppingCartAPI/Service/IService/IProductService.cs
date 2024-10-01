using Mango.Service.ShoppingCartAPI.Models.Dtos;

namespace Mango.Service.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
    }
}
