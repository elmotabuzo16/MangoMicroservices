﻿using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto> GetProductByIdAsync(int Id);
        Task<ResponseDto> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto> DeleteProductAsync(int Id);
    }
}
