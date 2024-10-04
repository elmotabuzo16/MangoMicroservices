using Mango.Services.RewardsAPI.Data;
using Mango.Services.RewardsAPI.Models;
using Mango.Services.RewardsAPI.Models.Dto;
using Mango.Services.RewardsAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.RewardsAPI.Services
{
    public class RewardsService : IRewardsService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public RewardsService(DbContextOptions<AppDbContext> dbOoptions)
        {
            _dbOptions = dbOoptions;
        }

        public async Task UpdateRewards(RewardsMessageDto rewardsMessageDto)
        {
            try
            {
                Rewards rewards = new Rewards()
                {
                    OrderId = rewardsMessageDto.OrderId,
                    RewardsActivity = rewardsMessageDto.RewardsActivity,
                    UserId = rewardsMessageDto.UserId,
                    RewardsDate = DateTime.Now,
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
            }

        }
    }
}
