using BusinessLogicLayer.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Validators
{
    

    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            //productID
            RuleFor(temp => temp.ProductId)
                .NotEmpty()
                .WithMessage("ProductId can't be blank");
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
