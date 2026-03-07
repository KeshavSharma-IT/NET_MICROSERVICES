using AutoMapper;
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrderMicroservice.BusinessLogicLayer.HttpClients;
using eCommerce.OrderMicroservice.BusinessLogicLayer.IServicesContracts;
using eCommerce.OrderMicroservice.DataAccessLayer.Entities;
using eCommerce.OrderMicroservice.DataAccessLayer.ReposittoryContracts;
using FluentValidation;
using FluentValidation.Results;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Services
{
    public class OrdersService : IOrdersServices
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OrderAddRequest> _orderaddValidator;
        private readonly IValidator<OrderItemAddRequest> _orderItemValidator;
        private readonly IValidator<OrderUpdateRequest> _orderUpdateValidator;
        private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateValidator;
        private readonly UserMicroserviceClient _userMicroserviceClient;
        private readonly ProductsMicroserviceClient _productsMicroserviceClient;
        

        public OrdersService(IOrdersRepository ordersRepository, IMapper mapper,IValidator<OrderAddRequest> addValidator,IValidator<OrderItemAddRequest> orderItemValidator, IValidator<OrderUpdateRequest> orderUpdateValidator,IValidator<OrderItemUpdateRequest> orderItemUpdateValidator, UserMicroserviceClient userMicroserviceClient, ProductsMicroserviceClient productsMicroserviceClient)
        {
            _mapper = mapper;
            _ordersRepository = ordersRepository;
            _orderaddValidator = addValidator;
            _orderUpdateValidator = orderUpdateValidator;
            _orderItemUpdateValidator= orderItemUpdateValidator;
            _orderItemValidator = orderItemValidator;
            _userMicroserviceClient = userMicroserviceClient;
            _productsMicroserviceClient= productsMicroserviceClient;
        }

        public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
        {
            List<ProductDTO?> products = new List<ProductDTO?>();

            if(orderAddRequest == null)
            {
                 throw new ArgumentNullException(nameof(orderAddRequest));
            }

            //using validator for validation
            ValidationResult validation=await _orderaddValidator.ValidateAsync(orderAddRequest);
            if (!validation.IsValid) {
                string errors = string.Join(", ", validation.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);
            }


            // above validator is not checking orderItesm so we are using another validator
            foreach(OrderItemAddRequest orderItemAddRequest in orderAddRequest.OrderItems)
            {
                ValidationResult validationitem = await _orderItemValidator.ValidateAsync(orderItemAddRequest);
                if (!validationitem.IsValid)
                {
                    string errors = string.Join(", ", validation.Errors.Select(temp => temp.ErrorMessage));
                    throw new ArgumentException(errors);

                }

                // check productID exists in produts microservices 

                ProductDTO? product= await   _productsMicroserviceClient.GetProductByProductID(orderItemAddRequest.ProductID);
                if(product == null)
                {
                    throw new ArgumentNullException("Invalid Product ID");
                }

                products.Add(product);
            }

            //but  as this service is all about order or it will have no data about user id
            //but we need to check user id as it is a part of order entity  so we need to validate

            UserDTO? user= await _userMicroserviceClient.GetUserByUserID(orderAddRequest.UserID);
            if (user == null) {
                
                throw new ArgumentNullException("Invalid User ID");
            }

            //


            //now using mapper to convert        OrderAddRequest to order
           Order orderinput= _mapper.Map<Order>(orderAddRequest);

            // calculate orderIteam price  for all item
            foreach (OrderItem orderItem in orderinput.OrderItems)
            {
                orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
            }
            //for all item calculating order Totalbill
            orderinput.TotalBill=orderinput.OrderItems.Sum(temp=>temp.TotalPrice);

           Order? AddedOrder= await _ordersRepository.AddOrder(orderinput);

            if (AddedOrder == null)
            {
                return null;
            }

            //now convert Order to OrderResponse

            OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(AddedOrder);


            if (addedOrderResponse != null)
            {

                foreach (OrderItemResponse orderItemResponse in addedOrderResponse.OrderItems)
                {
                    ProductDTO? product = products.Where(temp => temp.ProductID == orderItemResponse.ProductID).FirstOrDefault();
                    if (product == null)
                    {
                        continue;
                    }
                    _mapper.Map<ProductDTO, OrderItemResponse>(product, orderItemResponse);
                }
            }



            return addedOrderResponse;
                
                
                
         }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
           FilterDefinition<Order> filter= Builders<Order>.Filter.Eq(temp=>temp.OrderID, orderId);

            Order? exitingOrder=await  _ordersRepository.GetOrderByConditions(filter);

            if (exitingOrder == null) {                 
                return false;
            }

           bool isDeleted= await _ordersRepository.DeleteOrder(orderId);
           return true;

        }

        public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
           Order? order=await _ordersRepository.GetOrderByConditions(filter);
            if (order == null)
            {
                return null;
            }
           OrderResponse orderResponse= _mapper.Map<OrderResponse>(order);



            if (orderResponse != null)
            {

                foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
                {
                    ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);
                    if (product == null)
                    {
                        continue;
                    }
                    _mapper.Map<ProductDTO, OrderItemResponse>(product, orderItemResponse);
                }
            }

            return orderResponse;
        }

        public async Task<List<OrderResponse?>> GetOrders()
        {
            IEnumerable<Order?> order = await _ordersRepository.GetOrders();
            if (order == null)
            {
                return null;
            }
            IEnumerable<OrderResponse?> orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(order);

            foreach (OrderResponse? orderResponse in orderResponses)
            {
                if (orderResponse == null)
                {
                    continue;
                }

                foreach (OrderItemResponse orderItemResponse in orderResponse.OrderItems)
                {
                    ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItemResponse.ProductID);
                    if (product == null)
                    {
                        continue;
                    }
                        _mapper.Map<ProductDTO,OrderItemResponse>(product, orderItemResponse);
                }
            }

            return orderResponses.ToList();
        }

        public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
           IEnumerable<Order?> order = await _ordersRepository.GetOrdersByConditions(filter);
            if (order == null)
            {
                return null;
            }
            IEnumerable<OrderResponse?> orderResponse = _mapper.Map<IEnumerable<OrderResponse>>(order);
            return orderResponse.ToList();
        }

        public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
        {
            List<ProductDTO?> products = new List<ProductDTO?>();
            if (orderUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(orderUpdateRequest));
            }

            //using validator for validation
            ValidationResult validation = await _orderUpdateValidator.ValidateAsync(orderUpdateRequest);
            if (!validation.IsValid)
            {
                string errors = string.Join(", ", validation.Errors.Select(temp => temp.ErrorMessage));
                throw new ArgumentException(errors);
            }



            // above validator is not checking orderItesm so we are using another validator
            foreach (OrderItemUpdateRequest orderItemUpdateRequest in orderUpdateRequest.OrderItems)
            {
                ValidationResult validationitem = await _orderItemUpdateValidator.ValidateAsync(orderItemUpdateRequest);
                if (!validationitem.IsValid)
                {
                    string errors = string.Join(", ", validation.Errors.Select(temp => temp.ErrorMessage));
                    throw new ArgumentException(errors);

                }
                // check productID exists in produts microservices 

                ProductDTO? product = await _productsMicroserviceClient.GetProductByProductID(orderItemUpdateRequest.ProductID);
                if (product == null)
                {
                    throw new ArgumentNullException("Invalid Product ID");
                }
                products.Add(product);
            }

            //but  as this service is all about order or it will have no data about user id
            //but we need to check user id as it is a part of order entity


            // checking user ID is valid or not
            UserDTO user = await _userMicroserviceClient.GetUserByUserID(orderUpdateRequest.UserID);
            if (user == null)
            {

                throw new ArgumentNullException("Invalid User ID");
            }

            //now using mapper to convert OrderUpdateRequest to order
            Order orderinput = _mapper.Map<Order>(orderUpdateRequest);

            // calculate orderIteam price  for all item
            foreach (OrderItem orderItem in orderinput.OrderItems)
            {
                orderItem.TotalPrice = orderItem.UnitPrice * orderItem.Quantity;
            }
            //for all item calculating order Totalbill
            orderinput.TotalBill = orderinput.OrderItems.Sum(temp => temp.TotalPrice);

            Order? UpdatedOrder = await _ordersRepository.UpdateOrder(orderinput);

            if (UpdatedOrder == null)
            {
                return null;
            }

            //now convert Order to OrderResponse

            OrderResponse updatedOrderResponse = _mapper.Map<OrderResponse>(UpdatedOrder);

            if (updatedOrderResponse != null)
            {

                foreach (OrderItemResponse orderItemResponse in updatedOrderResponse.OrderItems)
                {
                    ProductDTO? product = products.Where(temp => temp.ProductID == orderItemResponse.ProductID).FirstOrDefault();
                    if (product == null)
                    {
                        continue;
                    }
                    _mapper.Map<ProductDTO, OrderItemResponse>(product, orderItemResponse);
                }
            }


            return updatedOrderResponse;
        }
    }
}
