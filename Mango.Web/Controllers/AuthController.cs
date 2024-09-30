using Mango.Web.Models;
using Mango.Web.Models.Auth;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
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

                await SignInUser(model);

                // Set the token to cookies
                _tokenProvider.SetToken(model.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response.Message;
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
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));


            var principal = new ClaimsPrincipal(identity);

            // This will define that the cookie is authenticated, and we can use [Authorize]
            // As well as User.Identity.IsAuthenticated from _Layout.cshtml
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
