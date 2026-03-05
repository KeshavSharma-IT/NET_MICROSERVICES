
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using System.Net.Http.Json;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.HttpClients
{
    public class UserMicroserviceClient
    {
        private readonly HttpClient _httpClient;

        public UserMicroserviceClient(HttpClient httpClient)
        {
             _httpClient = httpClient;
        }

        public async Task<UserDTO?> GetUserByUserID(Guid userID)
        {
            HttpResponseMessage responseMessage= await _httpClient.GetAsync($"/api/users/{userID}");
            if (!responseMessage.IsSuccessStatusCode)
            {
                return null;
            }

           UserDTO? user=await responseMessage.Content.ReadFromJsonAsync<UserDTO?>();

            if (user == null) {

                throw new ArgumentException("Invalid user ID");
            }

            return user;
        }
    }
}
