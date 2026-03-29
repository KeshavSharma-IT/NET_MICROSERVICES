using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.RabbitMQ
{
    public class RabbitMQProductDeletionConsumer : IDisposable  , IRabbitMQProductDeletionConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMQProductDeletionConsumer> _logger;
        private readonly IDistributedCache _distributedCache;

        public RabbitMQProductDeletionConsumer(IConfiguration configuration, ILogger<RabbitMQProductDeletionConsumer> logger, IDistributedCache distributedCache)
        {
            _configuration = configuration;
            _logger = logger;
            _distributedCache = distributedCache;

            Console.WriteLine($"RabbitMQ_HostName:{_configuration["RabbitMQ_HostName"]}");
            Console.WriteLine($"RabbitMQ_UserName:{_configuration["RabbitMQ_UserName"]}");
            Console.WriteLine($"RabbitMQ_Password:{_configuration["RabbitMQ_Password"]}");
            Console.WriteLine($"RabbitMQ_Port:{_configuration["RabbitMQ_Port"]}");

            string hostName = _configuration["RabbitMQ_HostName"]!;
            string userName = _configuration["RabbitMQ_UserName"]!;
            string password = _configuration["RabbitMQ_Password"]!;
            //string port = _configuration["RabbitMQ_Port"]!;
            int port = Convert.ToInt32(_configuration["RabbitMQ_Port"]);



            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = port,

            };
            _connection = connectionFactory.CreateConnection();

            _channel = _connection.CreateModel();
        }


        public void Consume()
        {
            //string routingKey = "ProductID.delete";
            var headers = new Dictionary<string, object>() {
                     { "x-match","all"},
                     {"event", "product.delete"},
                     {"RowCount",1 }
                };
            string queueName = "orders.product.delete.queue";

            //Create exchange
            string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
            //_channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Headers, durable: true);

            //create messsage queue
            _channel.QueueDeclare(queue:queueName,durable: true,exclusive:false,autoDelete:false,arguments:null);

            //Bind the message to exchane
            //_channel.QueueBind(queue:queueName,exchange:exchangeName,routingKey:routingKey);
            _channel.QueueBind(queue:queueName,exchange:exchangeName,routingKey:string.Empty,arguments:headers);

            //received message 

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async(sender, args) => 
            {
                byte[] body= args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                if (message != null) {
                    ProductDeletingMessage productDeletingMessage = JsonSerializer.Deserialize<ProductDeletingMessage>(message);

                    if (productDeletingMessage != null) {                     
                        _logger.LogInformation($"Product deleted: {productDeletingMessage.ProductID}, and Product name:{productDeletingMessage.ProductName}");
                        await HandleProductDeletion(productDeletingMessage.ProductID);
                    }

                }
            };

            _channel.BasicConsume(queue: queueName, consumer: consumer,autoAck:true);

        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }

        private async Task HandleProductDeletion(Guid ProductID)
        {
           

            string cacheKeyToWrite = $"product:{ProductID}";
            await _distributedCache.RemoveAsync(cacheKeyToWrite);

            
        }
    }
}
