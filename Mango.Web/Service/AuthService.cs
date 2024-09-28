using Mango.Web.Models;
using Mango.Web.Models.Auth;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.AuthAPIBaseURL + "/api/auth/register",
                Data = registrationRequestDto
            });

            return result;
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.AuthAPIBaseURL + "/api/auth/login",
                Data = loginRequestDto
            });

            return result;
        }

        public async Task<ResponseDto> AssignRoleAsync(AssignRoleDto assignRoleDto)
        {
            var result = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                ApiUrl = StaticDetails.AuthAPIBaseURL + "/api/auth/assign-role",
                Data = assignRoleDto
            });

            return result;
        }
        
    }
}
