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
    public class RabbitMQProductNameUpdateConsumer : IDisposable, IRabbitMQProductNameUpdateConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly ILogger<RabbitMQProductNameUpdateConsumer> _logger;
        private readonly IDistributedCache _distributedCache;

        public RabbitMQProductNameUpdateConsumer(IConfiguration configuration, ILogger<RabbitMQProductNameUpdateConsumer> logger, IDistributedCache distributedCache)
        {
            _configuration = configuration;
            _logger = logger;
            _distributedCache= distributedCache;

            Console.WriteLine($"RabbitMQ_HostName:{_configuration["RabbitMQ_HostName"]}");
            Console.WriteLine($"RabbitMQ_UserName:{_configuration["RabbitMQ_UserName"]}");
            Console.WriteLine($"RabbitMQ_Password:{_configuration["RabbitMQ_Password"]}");
            Console.WriteLine($"RabbitMQ_Port:{_configuration["RabbitMQ_Port"]}");

            string hostName = _configuration["RabbitMQ_HostName"]!;
            string userName = _configuration["RabbitMQ_UserName"]!;
            string password = _configuration["RabbitMQ_Password"]!;
            string port = System.Environment.GetEnvironmentVariable("RabbitMQ_Port")!;


            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = Convert.ToInt32(port),

            };
            Console.WriteLine($"RabbitMQ_Port:{Convert.ToInt32(port)}");
            _connection = connectionFactory.CreateConnection();

            _channel = _connection.CreateModel();
        }


        public void Consume()
        {
            //string routingKey = "product.update.name";  // use for direct/fallout/topic exchange
            var headers = new Dictionary<string, object>() {
                     { "x-match","all"},
                     {"event", "product.update"},
                     {"RowCount",1 }
                };
            string queueName = "orders.product.update.name.queue";

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

            consumer.Received += async (sender, args) => 
            {
                byte[] body= args.Body.ToArray();
                string message = Encoding.UTF8.GetString(body);
                if (message != null) {
                    //ProductNameUpdateMessage productNameUpdateMessage= JsonSerializer.Deserialize<ProductNameUpdateMessage>(message);
                    ProductDTO? productNameUpdateMessage = JsonSerializer.Deserialize<ProductDTO>(message);

                    //_logger.LogInformation($"Product name update: {productNameUpdateMessage.ProductID}, and new name:{productNameUpdateMessage.NewName}");

                    //Update products cache
                  await  HandleProductUpdation(productNameUpdateMessage);
                }
            };

            _channel.BasicConsume(queue: queueName, consumer: consumer,autoAck:true);

        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }


        private async Task HandleProductUpdation (ProductDTO product)
        {
            string productJson = JsonSerializer.Serialize(product);

            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                 .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                 .SetSlidingExpiration(TimeSpan.FromSeconds(100));

            string cacheKeyToWrite = $"product:{product.ProductID}";
            await _distributedCache.SetStringAsync(cacheKeyToWrite, productJson, options);

            _logger.LogInformation($"Product name update: {product.ProductID}, and new name:{product.ProductName}");
        }
    }
}
