using Mango.Services.EmailAPI.Services;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Mango.Services.EmailAPI.Dto;

namespace Mango.Services.EmailConsumer.RabbitMQAuthConsumer
{
    public class RabbitMQCartConsumer : BackgroundService
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQCartConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

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
            _channel.QueueDeclare(_configuration.GetValue<string>
                ("TopicAndQueueNames:EmailShoppingCartQueue"), false, false, false, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var email = JsonConvert.DeserializeObject<CartDto>(content);
                HandleMessage(email).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_configuration.GetValue<string>
                ("TopicAndQueueNames:EmailShoppingCartQueue"), false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(CartDto cartDto)
        {
            // EmailRegister is to log only
            await _emailService.EmailCartAndLog(cartDto);
        }

    }
}
