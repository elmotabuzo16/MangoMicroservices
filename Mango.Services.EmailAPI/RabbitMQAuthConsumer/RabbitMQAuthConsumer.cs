
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace Mango.Services.EmailConsumer.RabbitMQAuthConsumer
{
    public class RabbitMQAuthConsumer : BackgroundService
    {
        private readonly string _hostName;
        private readonly string _username;
        private readonly string _password;

        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQAuthConsumer(IConfiguration configuration, EmailService emailService)
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
                ("TopicAndQueueNames:EmailRegisterQueue"), false, false, false, null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                String email = JsonConvert.DeserializeObject<string>(content);
                HandleMessage(email).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_configuration.GetValue<string>
                ("TopicAndQueueNames:EmailRegisterQueue"), false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(string email)
        {
            // EmailRegister is to log only
            await _emailService.EmailRegister(email);
        }
    }
}
