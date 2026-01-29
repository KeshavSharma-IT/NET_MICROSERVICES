using eCommerce.BusinessLogicLayer.DTO;
using FluentValidation;


namespace eCommerce.BusinessLogicLayer.Validators
{
    public class ProductAddRequestValidator   :AbstractValidator<ProductAddRequest>
    {
        public ProductAddRequestValidator()
        {
            //productName
            RuleFor(temp => temp.ProductName)
                .NotEmpty()
                .WithMessage("Product name can't be blank");
            //Category
            RuleFor(temp => temp.Category)
                .IsInEnum()
                .WithMessage("Category  can't be blank");
            //UnitPrice
            RuleFor(temp => temp.UnitPrice)
                .InclusiveBetween(0, double.MaxValue)
                .WithMessage($"Unit price should be between 0 to {double.Max}");
            //QuentityInStock
            RuleFor(temp => temp.QuantityInStock)
                .InclusiveBetween(0, int.MaxValue)
                .WithMessage($"Unit price should be between 0 to {int.Max}");



        }
        
    }
}
