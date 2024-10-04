using Mango.Services.EmailAPI.Dto;
using Mango.Services.EmailAPI.Models.Dto;

namespace Mango.Services.EmailAPI.Services.Interface
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task EmailRegister(string email);
        Task LogOrderPlaced(RewardsMessageDto rewardsMessageDto);
    }
}
