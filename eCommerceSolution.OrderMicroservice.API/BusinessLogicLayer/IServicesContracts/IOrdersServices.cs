using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrderMicroservice.DataAccessLayer.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.IServicesContracts
{
    public interface IOrdersServices
    {
                         /// <summary>
                         /// 
                         /// </summary>
                         /// <returns>IT  WILL RETURN LIST OF ALL ORDERrESPONCE</returns>
        Task<List<OrderResponse?>> GetOrders();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>It will return matching order as OrderResponce Objects</returns>
        Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter);

        /// <summary>
        ///  it will return single matching order as responce 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter);

        /// <summary>
        ///  it will add order  into collection using order repo 
        /// </summary>
        /// <param name="order"></param>
        /// <returns> returns orderResponce</returns>
        Task<OrderResponse?> AddOrder(OrderAddRequest order); 
        /// <summary>
        ///  it will update order  into collection using order repo 
        /// </summary>
        /// <param name="order"></param>
        /// <returns> returns orderResponce</returns>
        Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdate);

        /// <summary>
        ///  it will delete object and return true and false
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Task<bool> DeleteOrder(Guid orderId);

    }
}
