using Ecommerce.Core.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Core.Validators
{
    public class LoginRequestValidator :AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator() 
        {
            //Email
            RuleFor(temp => temp.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email formate");

            //password
            RuleFor(temp => temp.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
