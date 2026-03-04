using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Validators
{
    public class OrderItemAddRequestValidator   : AbstractValidator<OrderItemAddRequest>
    {
        public OrderItemAddRequestValidator()
        {
            RuleFor(temp => temp.ProductID).NotEmpty().WithErrorCode("Product ID can't be blank");
            RuleFor(temp => temp.UnitPrice).NotEmpty().WithErrorCode("Unit Price can't be blank").
                GreaterThan(0).WithErrorCode("Unit Price cant be less than or quual to 0");
            RuleFor(temp => temp.Quantity).NotEmpty().WithErrorCode("Quantity  can't be blank").
                GreaterThan(0).WithErrorCode("Quantity  can't be less than or quual to 0");
            
        }
    }
}
