using Mango.Services.RewardsAPI.Models.Dto;
using Mango.Services.RewardsAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.RewardsConsumer.Messaging.Fanout
{
    public class RewardsConsumer : BackgroundService
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;

        private readonly IConfiguration _configuration;
        private readonly RewardsService _rewardsService;
        private IConnection _connection;
        private IModel _channel;
        string queueName = "";

        public RewardsConsumer(IConfiguration configuration, RewardsService rewardsService)
        {
            _configuration = configuration;
            _rewardsService = rewardsService;

            _hostName = "localhost";
            _password = "guest";
            _username = "guest";

            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Password = _password,
                UserName = _username
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_configuration.GetValue<string>
                ("TopicAndQueueNames:OrderCreatedTopic"), ExchangeType.Fanout);
            queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic"), "");

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var email = JsonConvert.DeserializeObject<RewardsMessageDto>(content);
                HandleMessage(email).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(RewardsMessageDto rewardsMessage)
        {
            await _rewardsService.UpdateRewards(rewardsMessage);
        }


    }
}
