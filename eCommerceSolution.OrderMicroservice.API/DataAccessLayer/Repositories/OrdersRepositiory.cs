
using eCommerce.OrderMicroservice.DataAccessLayer.Entities;
using eCommerce.OrderMicroservice.DataAccessLayer.ReposittoryContracts;
using MongoDB.Driver;
using static System.Net.WebRequestMethods;

namespace eCommerce.OrderMicroservice.DataAccessLayer.Repositories
{
    public class OrdersRepositiory : IOrdersRepository
    {
        private readonly IMongoCollection<Order> _orders;

        private readonly string collectionName = "orders";

        public OrdersRepositiory(IMongoDatabase mongoDatabase)
        {
            _orders = mongoDatabase.GetCollection<Order>(collectionName);
        }
        public async Task<Order?> AddOrder(Order order)
        {
            order.OrderID=Guid.NewGuid();

            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
         FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp=>temp.OrderID,orderId);
           Order? exitingOrder= (await _orders.FindAsync(filter)).FirstOrDefault();
            if (exitingOrder != null) {                  
                return false;
            }
            DeleteResult deleteResult= await _orders.DeleteOneAsync(filter);

            return deleteResult.DeletedCount > 0;

        }

        public async Task<Order?> GetOrderByConditions(FilterDefinition<Order> filter)
        {
                   return (await _orders.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return (await _orders.FindAsync(Builders<Order>.Filter.Empty)).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersByConditions(FilterDefinition<Order> filter)
        {
            return (await _orders.FindAsync(filter)).ToList();
        }

        public async Task<Order?> UpdateOrder(Order order)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, order.OrderID);
            Order? exitingOrder = (await _orders.FindAsync(filter)).FirstOrDefault();
            if (exitingOrder != null)
            {
                return null;
            }

             ReplaceOneResult replaceOne= await _orders.ReplaceOneAsync(filter, order);

            return order;


        }
    }
}
