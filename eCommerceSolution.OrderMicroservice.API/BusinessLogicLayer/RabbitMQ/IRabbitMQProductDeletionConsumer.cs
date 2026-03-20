namespace eCommerce.OrderMicroservice.BusinessLogicLayer.RabbitMQ
{
    public interface IRabbitMQProductDeletionConsumer
    {
        void Consume();
        void Dispose();
    }
}