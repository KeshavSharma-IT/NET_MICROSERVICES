using eCommerce.BusinessLogicLayer.IServices;
using eCommerce.BusinessLogicLayer.Mappers;
using eCommerce.BusinessLogicLayer.Services;
using eCommerce.BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {

            // Add data access Layer services into the ioc container

            services.AddAutoMapper(typeof(ProductAddPequestToProductMappingProfile).Assembly);  
            // as we are refering entire assembly  we dont need to register all mapper one will enough for all


            services.AddScoped<IProductServices,ProductService>();

            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();  //REGISTER VALIDATORS ONLY 1 REGESTER WILL WORK FOR ALL
            return services;
        }
    }
}
