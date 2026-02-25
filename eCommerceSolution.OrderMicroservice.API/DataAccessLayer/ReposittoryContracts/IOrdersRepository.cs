using eCommerce.OrderMicroservice.DataAccessLayer.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.DataAccessLayer.ReposittoryContracts
{
    public interface IOrdersRepository
    {

        /// <summary>
        /// Retrieves all Orders asynchronously
        /// </summary>
        /// <returns>  Returns all orders from the orders collection
        /// </returns>
        Task<IEnumerable<Order>> GetOrders();
         
        /// <summary>
        /// Retrieves a collection of orders that satisfy the specified filter criteria.
        /// </summary>
        /// <remarks>This method is asynchronous and should be awaited. Ensure that the filter is properly
        /// constructed to avoid unexpected results.</remarks>
        /// <param name="filter">The filter definition used to determine which orders to retrieve. This parameter cannot be null and must
        /// specify valid filtering conditions.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of
        /// orders matching the filter criteria. The collection will be empty if no orders meet the conditions.</returns>
        Task<IEnumerable<Order>> GetOrdersByConditions(FilterDefinition<Order> filter);

        /// <summary>
        /// Asynchronously retrieves a single order that matches the specified filter criteria.
        /// </summary>
        /// <remarks>This method may involve database access, which can affect performance. Ensure that
        /// the filter is properly defined to avoid unexpected results.</remarks>
        /// <param name="filter">The filter definition used to specify the conditions for selecting the order. This parameter cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an instance of the Order class
        /// that matches the filter criteria, or null if no matching order is found.</returns>
        Task<Order?> GetOrderByConditions(FilterDefinition<Order> filter);

        /// <summary>
        /// Adds a new order to the data store asynchronously.
        /// </summary>
        /// <remarks>Ensure that the order object is valid and meets all required criteria before calling
        /// this method. The method does not guarantee persistence if validation fails.</remarks>
        /// <param name="order">The order object containing the details of the order to be added. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created order object if the
        /// addition succeeds; otherwise, null.</returns>
        Task<Order?> AddOrder(Order order);
        /// <summary>
        /// Updates the specified order with new information asynchronously and returns the updated order if the
        /// operation succeeds.
        /// </summary>
        /// <remarks>This method may throw exceptions if the provided order is invalid or if an error
        /// occurs during the update process.</remarks>
        /// <param name="order">The order object containing the updated details to apply. Must not be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated order if the update
        /// is successful; otherwise, null if the order could not be found.</returns>

        Task<Order?> UpdateOrder(Order order);

        /// <summary>
        /// it will delete order basesd on orderId and return true or false
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<bool> DeleteOrder(Guid orderId);



    }
}
