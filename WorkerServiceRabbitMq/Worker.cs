using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Taufiqurrahman_Test_Qoin.EntityFramework;
using Taufiqurrahman_Test_Qoin.ServiceWorker.Models;

namespace WorkerServiceRabbitMq
{

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ConnectionFactory _conFact { get;set; }
        private IConnection _connection { get;set; }
        private IModel _model { get;set; }
        private const string _queueName = "qtest1";
        private const string _host = "localhost";
        private const int _port = 5672;
        private const string _username = "guest";
        private const string _password = "guest";
        private Test01s _ctx { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _ctx = new Test01s(new AppDbContext());
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            string rabbitHostName = Environment.GetEnvironmentVariable("RABBIT_HOSTNAME");
            _conFact = new ConnectionFactory
            {
                HostName = rabbitHostName ?? _host,
                Port = _port,
                UserName = _username,
                Password = _password,
                DispatchConsumersAsync = true
            };
            _connection = _conFact.CreateConnection();
            _model = _connection.CreateModel();
            _model.QueueDeclarePassive(_queueName);
            _model.BasicQos(0, 1, false);
            _logger.LogInformation($"Queue [{_queueName}] is waiting for messages.");

            return base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (bc, arg) =>
             {
                 string message = Encoding.UTF8.GetString(arg.Body.ToArray());
                 _logger.LogInformation($"Processing msg: '{message}'.");
                 try
                 {
                     Message order = JsonSerializer.Deserialize<Message>(message);
                     
                     if(order != null)
                     {
                         _logger.LogInformation($"Success Deserialize Message with command '{order.command}'");
                         if (order.data != null)
                         {
                             if (_ctx.CheckDbConnection())
                             {


                                 _logger.LogInformation($"order data not null process command '{order.command}'");

                                 if (order.command == "create")
                                 {
                                     if (!_ctx.CheckDataExist(order.data.Nama))
                                     {
                                         _logger.LogInformation($"Creating new data to maria db.");
                                         if (_ctx.InsertTest01(order.data))
                                         {
                                             _logger.LogInformation($"Succes create new data to maria db");
                                         }
                                         else
                                         {
                                             _logger.LogInformation($"Failed create new data to maria db");
                                         }
                                     }
                                     else _logger.LogInformation($"Data already exist. No data created.");

                                 }
                                 else if (order.command == "update")
                                 {
                                     if (_ctx.CheckDataExist(order.data.Nama))
                                     {
                                         _logger.LogInformation($"Updating existing data to maria db");
                                         if (_ctx.UpdateTest01(order.data))
                                         {
                                             _logger.LogInformation($"Succes update existing data to maria db");
                                         }
                                         else _logger.LogInformation($"Failed update data to maria db");
                                     }
                                     else _logger.LogInformation($"Data not exist. No data updated");


                                 }
                                 else if (order.command == "delete")
                                 {
                                     if (_ctx.CheckDataExist(order.data.Nama))
                                     {
                                         _logger.LogInformation($"Deleting existing data from maria db");
                                         if (_ctx.DeleteTest01(order.data.Nama))
                                         {
                                             _logger.LogInformation($"Succes delete existing data from maria db");
                                         }
                                         else _logger.LogInformation($"Failed delete data from maria db");
                                     }
                                     else _logger.LogInformation($"Data not exist. No data deleted");

                                 }
                             }
                             else
                             {
                                 _logger.LogInformation($"Cant connect to maria db.");
                             }
                         }
                         else _logger.LogInformation($"Response message is null");

                     }
                     //_logger.LogInformation($"");

                     await Task.Delay(new Random().Next(1, 3) * 1000, stoppingToken); // simulate an async email process

                   
                     _model.BasicAck(arg.DeliveryTag, false);
                 }
                 catch (JsonException)
                 {
                     _logger.LogError($"JSON Parse Error: '{message}'.");
                     _model.BasicNack(arg.DeliveryTag, false, false);
                 }
                 catch (AlreadyClosedException)
                 {
                     _logger.LogInformation("RabbitMQ is closed!");
                 }
                 catch (Exception e)
                 {
                     _logger.LogError(default, e, e.Message);
                 }
             };
            _model.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            await Task.CompletedTask;
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();
            _logger.LogInformation("RabbitMQ connection is closed.");
        }
    }
}