using Mango.Services.RewardsAPI.Models.Dto;

namespace Mango.Services.RewardsAPI.Services.Interface
{
    public interface IRewardsService
    {
        Task UpdateRewards(RewardsMessageDto rewardsMessage);
    }
}
