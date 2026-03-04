using AutoMapper;
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
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
        

        public OrdersService(IOrdersRepository ordersRepository, IMapper mapper,IValidator<OrderAddRequest> addValidator,IValidator<OrderItemAddRequest> orderItemValidator, IValidator<OrderUpdateRequest> orderUpdateValidator,IValidator<OrderItemUpdateRequest> orderItemUpdateValidator)
        {
           _mapper = mapper;
            _ordersRepository = ordersRepository;
            _orderaddValidator = addValidator;
            _orderUpdateValidator = orderUpdateValidator;
            _orderItemUpdateValidator= orderItemUpdateValidator;
            _orderItemValidator = orderItemValidator;
        }

        public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
        {
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
            }

            //but  as this service is all about order or it will have no data about user id
            //but we need to check user id as it is a part of order entity



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
            return orderResponse;
        }

        public async Task<List<OrderResponse?>> GetOrders()
        {
            IEnumerable<Order?> order = await _ordersRepository.GetOrders();
            if (order == null)
            {
                return null;
            }
            IEnumerable<OrderResponse?> orderResponse = _mapper.Map<IEnumerable<OrderResponse>>(order);
            return orderResponse.ToList();
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
            }

            //but  as this service is all about order or it will have no data about user id
            //but we need to check user id as it is a part of order entity



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

            OrderResponse addedOrderResponse = _mapper.Map<OrderResponse>(UpdatedOrder);

            return addedOrderResponse;
        }
    }
}
