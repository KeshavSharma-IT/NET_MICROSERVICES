
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.Bulkhead;
using System.Net.Http.Json;
using System.Text.Json;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.HttpClients
{
    public class ProductsMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsMicroserviceClient> _logger;
        private readonly IDistributedCache _distributedCache;

        public ProductsMicroserviceClient(HttpClient httpClient, ILogger<ProductsMicroserviceClient> logger, IDistributedCache distributedCache)
        {
             _httpClient = httpClient;
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public async Task<ProductDTO?> GetProductByProductID(Guid productID)
        {
            //HttpResponseMessage responseMessage= await _httpClient.GetAsync($"/api/products/search/productId/{productID}");
            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    return null;
            //}

            try
            {
                //using cache before hitting api
                string cacheKey = $"product:{productID}";
                string? cachedProduct= await _distributedCache.GetStringAsync(cacheKey) ;

                if (cachedProduct!=null)
                {
                    ProductDTO? cachedproduct=  JsonSerializer.Deserialize<ProductDTO>(cachedProduct);
                    return cachedproduct;
                }

                // if data not found hitting api

                HttpResponseMessage response = await _httpClient.GetAsync($"/api/products/search/productId/{productID}");
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        ProductDTO? productFromFallBack = await response.Content.ReadFromJsonAsync<ProductDTO?>();

                        if (productFromFallBack == null)
                        {

                            throw new NotImplementedException("fallBack policy was not implemented");
                        }
                        return productFromFallBack;
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        throw new HttpRequestException("Bad request", null, System.Net.HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        throw new HttpRequestException($"Http request failed with status code {response.StatusCode}");
                    }
                }

                ProductDTO? product = await response.Content.ReadFromJsonAsync<ProductDTO?>();

                if (product == null)
                {

                    throw new ArgumentException("Invalid product ID");
                }

                // adding product in cache
                string productJson= JsonSerializer.Serialize(product);

               DistributedCacheEntryOptions options= new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(100));

                string cacheKeyToWrite = $"product:{productID}";
                await _distributedCache.SetStringAsync(cacheKeyToWrite, productJson,options);

                return product;
            }
            catch (BulkheadRejectedException ex)
            {
                _logger.LogError(ex, "Bulkhead isolation blocks the request since the request queue is full");

                return new ProductDTO(
                  ProductID: Guid.NewGuid(),
                  ProductName: "Temporarily Unavailable (Bulkhead)",
                  Category: "Temporarily Unavailable (Bulkhead)",
                  UnitPrice: 0,
                  QuantityInStock: 0);
            }

        }
    }
}
