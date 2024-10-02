using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

		[HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ProductIndex()
        {
			var list = new List<ProductDto>();

			var response = await _productService.GetAllProductsAsync();

			if (response != null && response.IsSuccess)
			{
                // Deserializing because you want to see the values of the object from View
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
			}
			else
			{
				TempData["error"] = response?.Message;
			}

			return View(list);
		}

		[HttpGet]
		public async Task<IActionResult> ProductCreate()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ProductCreate(ProductDto model)
		{
			if (ModelState.IsValid)
			{
				var response = await _productService.CreateProductAsync(model);

				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Product created successfully";
					return RedirectToAction(nameof(ProductIndex));
				}
				else
				{
					TempData["error"] = response?.Message;
				}
				
			}

			return View(model);
		}

        [HttpGet]
        public async Task<IActionResult> ProductEdit()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                // Deserializing because you want to see the values of the object from View
                var model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            var response = await _productService.DeleteProductAsync(productDto.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }
    }
}
