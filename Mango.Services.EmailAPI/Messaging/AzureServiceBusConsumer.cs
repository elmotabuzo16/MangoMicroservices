using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Dto;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Mango.Services.EmailAPI.Services.Interface;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBuSconnectionString;
        private readonly string emailCartQueue;
        private readonly string emailRegister;
        private readonly string OrderCreatedTopic;
        private readonly string OrderCreated_Rewards_Subscription;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _emailOrderProcessor;
        private ServiceBusProcessor _emailRegisterProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            serviceBuSconnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            // Queues
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            emailRegister = _configuration.GetValue<string>("TopicAndQueueNames:EmailRegisterQueue");

            // Topics
            OrderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            OrderCreated_Rewards_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");


            var client = new ServiceBusClient(serviceBuSconnectionString);

            // Queues Processor
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _emailRegisterProcessor = client.CreateProcessor(emailRegister);

            // Topics Processor
            _emailOrderProcessor = client.CreateProcessor(OrderCreatedTopic, OrderCreated_Rewards_Subscription);

        }

        public async Task Start()
        {
            // For shopping cart
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            // For register email
            _emailRegisterProcessor.ProcessMessageAsync += OnEmailRegisterReceived;
            _emailRegisterProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailRegisterProcessor.StartProcessingAsync();

            _emailOrderProcessor.ProcessMessageAsync += OnOrderPlacedRequestReceived;
            _emailOrderProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderProcessor.StartProcessingAsync();
        }

        private async Task OnOrderPlacedRequestReceived(ProcessMessageEventArgs args)
        {
            // this is where you will receive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<RewardsMessageDto>(body);

            try
            {
                // Log email
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnEmailRegisterReceived(ProcessMessageEventArgs args)
        {
            // this is where you will receive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<string>(body);

            try
            {
                // Log email
                await _emailService.EmailRegister(objMessage);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            // this is where you will receive message
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var objMessage = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                // Log email
                await _emailService.EmailCartAndLog(objMessage);
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
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _emailRegisterProcessor.StopProcessingAsync();
            await _emailRegisterProcessor.DisposeAsync();

            await _emailOrderProcessor.StopProcessingAsync();
            await _emailOrderProcessor.DisposeAsync();
            
        }
    }
}
