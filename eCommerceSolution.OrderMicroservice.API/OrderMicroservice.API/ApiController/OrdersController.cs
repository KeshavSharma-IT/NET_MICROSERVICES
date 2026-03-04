using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrderMicroservice.BusinessLogicLayer.IServicesContracts;
using eCommerce.OrderMicroservice.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;

namespace OrderMicroservice.API.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersServices _ordersServices;
        public OrdersController(IOrdersServices ordersServices)
        {
            _ordersServices = ordersServices;
        }

        //Get  /api/get
        [HttpGet]
        public async Task<IEnumerable<OrderResponse?>> Get()
        {
            List<OrderResponse?> ordersResponses= await _ordersServices.GetOrders();
            Console.WriteLine("COUNT = " + ordersResponses.Count);
            return ordersResponses;
            //return (IEnumerable<OrderResponse?>)Ok(ordersResponses);
        }

        //Get  /api/Orders/search/orderId/{orderID}
        [HttpGet("search/orderId/{orderID}")]
        public async Task<OrderResponse?> GetOrderByOrderID(Guid orderID)
        {
            FilterDefinition<Order> filter=Builders<Order>.Filter.Eq(temp=>temp.OrderID, orderID);
            OrderResponse? orderResponses = await _ordersServices.GetOrderByCondition(filter);
            return orderResponses;
        }


        //Get  /api/Orders/search/productID/{productID}
        [HttpGet("search/productId/{productID}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductID(Guid productID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(temp => temp.OrderItems, Builders<OrderItem>.Filter.Eq(tempProduct=>tempProduct.ProductID, productID));
           List<OrderResponse?> ordersResponses = await _ordersServices.GetOrdersByCondition(filter);
            return ordersResponses;
        }

        //Get  /api/Orders/search/orderDate/{orderDate}
        [HttpGet("search/orderDate/{orderDate}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime orderDate)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp=>temp.OrderDate.ToString("yyyy-MM-dd"),orderDate.ToString("yyyy-MM-dd"));
            List<OrderResponse?> ordersResponses = await _ordersServices.GetOrdersByCondition(filter);
            return ordersResponses;
        }

        //Get  /api/Orders/search/userid/{userID}
        [HttpGet("search/userid/{userID}")]
        public async Task<IEnumerable<OrderResponse?>> GetOrdersByuserID(Guid userID)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.UserID, userID);
            List<OrderResponse?> ordersResponses = await _ordersServices.GetOrdersByCondition(filter);
            return ordersResponses;
        }

        //post /api/Orders
        [HttpPost()]
        public async Task<IActionResult> Post(OrderAddRequest orderAddRequest)
        {
            if(orderAddRequest == null)
            {
                return BadRequest("Invalid order data");
            }
           OrderResponse? orderResponse= await _ordersServices.AddOrder(orderAddRequest);

            if (orderResponse == null)
            {
                return Problem("Error in adding order");
            }

            return Created($"api/Orders/search/orderid/{orderResponse.OrderID}",orderResponse);   
        }

        //put /api/Orders/{orderID}
        [HttpPut("{orderID}")]
        public async Task<IActionResult> Put(Guid orderID, OrderUpdateRequest orderUpdateRequest)
        {
            if (orderUpdateRequest == null)
            {
                return BadRequest("Invalid order data");
            }

            if(orderID != orderUpdateRequest.OrderID)
            {
                return BadRequest("Order Id in URLdose't match with orderId in the Request body");
            }


            OrderResponse? orderResponse = await _ordersServices.UpdateOrder(orderUpdateRequest);

            if (orderResponse == null)
            {
                return Problem("Error in updating order");
            }

            return Ok(orderResponse);
        }

        //delete /api/Orders/{orderID}
        [HttpDelete("{orderID}")]
        public async Task<IActionResult> Delete(Guid orderID)
        {
            if (orderID == Guid.Empty)
            {
                return BadRequest("Order ID is blank");
            }
            bool isDeleted = await _ordersServices.DeleteOrder(orderID);

            if (!isDeleted)
            {
                return Problem("Error in deleteing order");
            }

            return Ok(isDeleted);
        }




    }
}
