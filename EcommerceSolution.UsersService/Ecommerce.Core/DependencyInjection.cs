using Ecommerce.Core.IService;
using Ecommerce.Core.Services;
using Ecommerce.Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ecommerce.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension Method to add core servicess to dependency injection container
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IServiceCollection AddCore(this IServiceCollection service)
    {
        service.AddTransient<IUsersService, UserService>();

       
        service.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

        return service;
    }

}
