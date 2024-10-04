using Azure.Messaging.ServiceBus;
using Mango.Services.RewardsAPI.Models;
using Mango.Services.RewardsAPI.Models.Dto;
using Mango.Services.RewardsAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.RewardsAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBuSconnectionString;
        private readonly string OrderCreatedTopic;
        private readonly string OrderCreated_Rewards_Subscription;

        private readonly IConfiguration _configuration;
        private readonly RewardsService _rewardsMessage;

        private ServiceBusProcessor _rewardsProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, RewardsService rewardsMessage)
        {
            _configuration = configuration;
            _rewardsMessage = rewardsMessage;
            serviceBuSconnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            // Topics
            OrderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            OrderCreated_Rewards_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");

            var client = new ServiceBusClient(serviceBuSconnectionString);

            // Topics Processor
            _rewardsProcessor = client.CreateProcessor(OrderCreatedTopic, OrderCreated_Rewards_Subscription);
        }

        public async Task Start()
        {
            // For rewards
            _rewardsProcessor.ProcessMessageAsync += OnOrderedRewards;
            _rewardsProcessor.ProcessErrorAsync += ErrorHandler;


            await _rewardsProcessor.StartProcessingAsync();
        }

        private async Task OnOrderedRewards(ProcessMessageEventArgs args)
        {
            // this is where you will receive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<RewardsMessageDto>(body);

            try
            {
                // Call the service
                await _rewardsMessage.UpdateRewards(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // If error occurs, you can send email to admin
            // But for this, Console.Write is fine
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
            await _rewardsProcessor.StopProcessingAsync();
            await _rewardsProcessor.DisposeAsync();
        }
    }
}
