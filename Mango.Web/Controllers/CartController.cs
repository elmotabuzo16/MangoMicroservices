using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            // Get the user id from logged in user
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?
                .FirstOrDefault()?.Value;

            var response = await _cartService.RemoveCartAsync(cartDetailsId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var response = await _cartService.RemoveCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            var cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Email)?
                .FirstOrDefault()?.Value;

            var response = await _cartService.EmailCartAsync(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed shortly";
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            var cart = await LoadCartDtoBasedOnLoggedInUser();
            cart.CartHeader.FirstName = cartDto.CartHeader.FirstName;
            cart.CartHeader.LastName = cartDto.CartHeader.LastName;
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;

            var response = await _orderService.CreateOrder(cart);

            var orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if (response != null && response.IsSuccess)
            {
            }

            return View();
        }


        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            // Get the user id from logged in user
            var userId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?
                .FirstOrDefault()?.Value;

            var response = await _cartService.GetCartByUserIdAsync(userId);

            if (response.Result != null && response.IsSuccess)
            {
                var cartDto = JsonConvert.DeserializeObject<CartDto>(response.Result.ToString());
                return cartDto;
            }

            return new CartDto();
        }

    }
}
