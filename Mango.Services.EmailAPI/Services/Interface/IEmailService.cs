using Mango.Services.EmailAPI.Dto;

namespace Mango.Services.EmailAPI.Services.Interface
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task EmailRegister(string email);
    }
}
