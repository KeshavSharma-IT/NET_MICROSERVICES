using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using FluentValidation;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Validators
{
    public class OrderItemUpdateRequestValidator : AbstractValidator<OrderItemUpdateRequest>
    {
        public OrderItemUpdateRequestValidator()
        {
            RuleFor(temp => temp.ProductID).NotEmpty().WithErrorCode("Product ID can't be blank");
            RuleFor(temp => temp.UnitPrice).NotEmpty().WithErrorCode("Unit Price can't be blank").
                GreaterThan(0).WithErrorCode("Unit Price cant be less than or equal to 0");
            RuleFor(temp => temp.Quantity).NotEmpty().WithErrorCode("Quantity  can't be blank").
                GreaterThan(0).WithErrorCode("Quantity  can't be less than or equal to 0");
        }
    }
}
