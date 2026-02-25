
using eCommerce.OrderMicroservice.DataAccessLayer.Entities;
using eCommerce.OrderMicroservice.DataAccessLayer.ReposittoryContracts;
using MongoDB.Driver;

namespace eCommerce.OrderMicroservice.DataAccessLayer.Repositories
{
    public class OrdersRepositiory : IOrdersRepository
    {
        Task<Order?> IOrdersRepository.AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        Task<bool> IOrdersRepository.DeleteOrder(Guid orderId)
        {
            throw new NotImplementedException();
        }

        Task<Order?> IOrdersRepository.GetOrderByConditions(FilterDefinition<Order> filter)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Order>> IOrdersRepository.GetOrders()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Order>> IOrdersRepository.GetOrdersByConditions(FilterDefinition<Order> filter)
        {
            throw new NotImplementedException();
        }

        Task<Order?> IOrdersRepository.UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
