using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Taufiqurrahman_Test_Qoin.EntityFramework.Models;

namespace Taufiqurrahman_Test_Qoin.EntityFramework.Repositories
{
    public class RabbitMqs : IRabbitMqs
    {
        private readonly RabbitMq _options;
        private IConnection _connection;
        private IModel _channel;
        public RabbitMqs(IOptions<RabbitMq> optionsAccs)
        {
            _options = optionsAccs.Value;
        }
        public void Publish<T>(T message, string exchangeName, string exchangeType,  string routeKey) where T : class
        {
            if (message == null)
                return;
                _channel.ExchangeDeclare(exchangeName,exchangeType, true, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _channel.BasicPublish(exchange: exchangeName,
                    routingKey: routeKey,
                    basicProperties: properties,
                    body: sendBytes);
            
        }

        public bool Connection()
        {
            if (_connection != null && _connection.IsOpen)
                return true;
            string rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
            var factory = new ConnectionFactory()
            {
                HostName = rabbitHostName??_options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                DispatchConsumersAsync = true
            };

          _connection=  factory.CreateConnection();
            return _connection.IsOpen;
        }

        public void CreateChanel()
        {
            if(_connection != null && _connection.IsOpen)
            {
                _channel = _connection.CreateModel();
               // _channel.QueueDeclarePassive(_queueName);
               // _channel.BasicQos(0, 1, false);
            }
        }
        public void CloseChannel()
        {
            if (!_channel.IsOpen)
            {
                if(_connection.IsOpen)
                _connection.Close();

                _channel?.Dispose();
            }
        }
    }
}
