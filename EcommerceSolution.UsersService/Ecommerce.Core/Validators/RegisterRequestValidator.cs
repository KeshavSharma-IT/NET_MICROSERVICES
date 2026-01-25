using Ecommerce.Core.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Core.Validators
{
    public class RegisterRequestValidator       :AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            // Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            // Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            // Person Name
            RuleFor(x => x.PersonName)
                .NotEmpty().WithMessage("Person name is required")
                .MinimumLength(5).WithMessage("Person name must be at least 2 characters")
                .MaximumLength(50).WithMessage("Person name must be at below 50 characters");

            // Gender
            RuleFor(x => x.Gender)
                .NotNull().WithMessage("Gender is required")
                .IsInEnum().WithMessage("Invalid gender value");
        }
    }
}
