using FluentValidation;
using eCommerce.OrderMicroservice.BusinessLogicLayer.IServicesContracts;
using eCommerce.OrderMicroservice.BusinessLogicLayer.Mappers;
using eCommerce.OrderMicroservice.BusinessLogicLayer.Services;
using eCommerce.OrderMicroservice.BusinessLogicLayer.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration) {
                     
            
            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
            services.AddAutoMapper(config =>
            {
                config.AddMaps(typeof(OrderAddRequestToOrderMappingProfile).Assembly);
            });
            services.AddScoped<IOrdersServices, OrdersService>();
            return services;
        }
    }
}
                                                                                                                