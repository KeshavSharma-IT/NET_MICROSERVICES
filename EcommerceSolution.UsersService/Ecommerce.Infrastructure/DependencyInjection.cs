using Ecommerce.Core.IRepository;
using Ecommerce.Infrastructure.DbContext;
using Ecommerce.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ecommerce.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension Method to add infrastructure servicess to dependency injection container
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection service)
    {

        service.AddTransient<IUsersRepository, UserRepositories>();
        service.AddTransient<DapperDbContext>();
        return service;
    }

}
