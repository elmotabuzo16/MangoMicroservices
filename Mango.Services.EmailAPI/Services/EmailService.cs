﻿using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Dto;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOoptions)
        {
            _dbOptions = dbOoptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");

            foreach (var item in cartDto.CartDetails)
            {
                message.AppendLine("<li>");
                message.AppendLine(item.Product.Name + " x " + item.Count);
                message.AppendLine("</li>");
            }

            message.Append("/<ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task EmailRegister(string email)
        {
            await LogAndEmail(email, email);
        }

        public async Task LogOrderPlaced(RewardsMessageDto rewardsMessageDto)
        {
            string message = $"New order placed, Order ID: {rewardsMessageDto.OrderId}";
            await LogAndEmail(message, "elmotabuzojr.ust@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new EmailLogger()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message,
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
