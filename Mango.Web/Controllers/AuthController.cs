using Mango.Web.Models;
using Mango.Web.Models.Auth;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            LoginRequestDto loginRequestDto = new LoginRequestDto();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var response = await _authService.LoginAsync(loginRequestDto);

            if (response != null && response.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                // This is added because from the view, we have asp-validation-summary=All
                ModelState.AddModelError("CustomError", response.Message);
                return View(loginRequestDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegistrationRequestDto registrationRequestDto = new RegistrationRequestDto();
            return View(registrationRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
            var response = await _authService.RegisterAsync(model);
            var assignRoleDto = new AssignRoleDto()
            {
                Role = "Customer",
                Username = model.Email
            };

            if (response != null && response.IsSuccess)
            {
                var roleResponse = await _authService.AssignRoleAsync(assignRoleDto);

                if (roleResponse != null && roleResponse.IsSuccess) 
                {
                    TempData["success"] = "Registration Successfully";
                    return RedirectToAction(nameof(Login));
                }
            }
            TempData["error"] = response.Message;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return View();
        }
    }
}
