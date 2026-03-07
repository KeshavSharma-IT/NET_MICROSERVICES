
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using System.Net.Http.Json;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.HttpClients
{
    public class ProductsMicroserviceClient
    {
        private readonly HttpClient _httpClient;

        public ProductsMicroserviceClient(HttpClient httpClient)
        {
             _httpClient = httpClient;
        }

        public async Task<ProductDTO?> GetProductByProductID(Guid productID)
        {
            HttpResponseMessage responseMessage= await _httpClient.GetAsync($"/api/products/search/productId/{productID}");
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

            ProductDTO? product=await responseMessage.Content.ReadFromJsonAsync<ProductDTO?>();

            if (product == null) {

                throw new ArgumentException("Invalid product ID");
            }

            return product;
        }
    }
}
