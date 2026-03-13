using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
using Polly.Fallback;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Policies
{
    public class ProductsMicroservicePolicies : IProductsMicroservicePolicies
    {
        private readonly ILogger<ProductsMicroservicePolicies> _logger;

        public ProductsMicroservicePolicies(ILogger<ProductsMicroservicePolicies> logger)
        {
            _logger= logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicyPolicy()
        {
            AsyncBulkheadPolicy<HttpResponseMessage> policy = Policy.BulkheadAsync<HttpResponseMessage>(maxParallelization: 2,   // allow up to 2 concurrent request
                maxQueuingActions: 40, // queuw up to 40 additional
                onBulkheadRejectedAsync: (Context) =>
                {
                    _logger.LogWarning("BulkheadIsolation triggered. Can't send any more request since the queue is full");

                    throw new BulkheadRejectedException("Bulkhead queue is full");
                }
                );
            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetFallBackPolicy()
        {
            AsyncFallbackPolicy<HttpResponseMessage> policy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                   .FallbackAsync(async (context) =>
                   {
                   _logger.LogInformation("FallBack triggered: The request failed, returning dummy data");
                   ProductDTO product = new ProductDTO(ProductID: Guid.Empty, ProductName: "Temproraily Unavailable (fallback)", UnitPrice: 0, QuantityInStock: 0, Category: "Temproraily Unavailable (fallback)");

                       var response = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                       {
                           Content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json")
                       };

                       return response;
                   });
            return policy;
        }
    }
}
