using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.RabbmitMQSender;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IRabbmitMQAuthMessageSender _rabbmitMQAuthMessageSender;
        private readonly ResponseDto _responseDto;

        public AuthApiController(IAuthService authService, IConfiguration configuration, IRabbmitMQAuthMessageSender rabbmitMQAuthMessageSender)
        {
            _authService = authService;
            _configuration = configuration;
            _rabbmitMQAuthMessageSender = rabbmitMQAuthMessageSender;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await _authService.Register(registrationRequestDto);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = errorMessage;
                return BadRequest(_responseDto);
            }

            _rabbmitMQAuthMessageSender.SendMessage(registrationRequestDto.Email, _configuration.GetValue<string>("TopicAndQueueNames:EmailRegisterQueue"));

            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);

            if (loginResponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is incorrect";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDto model)
        {
            var assignRole = await _authService.AssignRole(model.Username, model.Role.ToUpper());

            if (assignRole != true)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error encountered";
                return BadRequest(_responseDto);
            }

            return Ok(_responseDto);
        }

    }
}
