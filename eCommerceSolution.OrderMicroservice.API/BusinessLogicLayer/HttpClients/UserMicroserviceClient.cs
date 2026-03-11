
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net.Http.Json;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.HttpClients
{
    public class UserMicroserviceClient
    {
        private readonly HttpClient _httpClient;

        private readonly ILogger<UserMicroserviceClient> _logger;
        public UserMicroserviceClient(HttpClient httpClient, ILogger<UserMicroserviceClient> logger)
        {
             _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<UserDTO?> GetUserByUserID(Guid userID)
        {
            //HttpResponseMessage responseMessage= await _httpClient.GetAsync($"/api/users/{userID}");
            //if (!responseMessage.IsSuccessStatusCode)
            //{
            //    return null;
            //}

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"/api/users/{userID}");

                if (!response.IsSuccessStatusCode)
                {
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
                        //throw new HttpRequestException($"Http request failed with status code {response.StatusCode}");

                        //  creating fault data 

                        return new UserDTO(PersonName: "Temporarily Unavailable", Email: "Temporarily Unavailable", Gender: "Temporarily Unavailable", UserID: Guid.Empty);
                    }
                }


                    UserDTO? user = await response.Content.ReadFromJsonAsync<UserDTO?>();

                    if (user == null)
                    {

                        throw new ArgumentException("Invalid user ID");
                    }

                    return user;

            }   catch(BrokenCircuitException ex)
            {
                _logger.LogError(ex,"Request failed because of circuit breaker, it  is Open state. Returning dummy data");
                return new UserDTO(PersonName: "Temporarily Unavailable", Email: "Temporarily Unavailable", Gender: "Temporarily Unavailable", UserID: Guid.Empty);
            }
            catch (TimeoutRejectedException ex)
            {
                _logger.LogError(ex, "Timeout occurred while fetching user data. Returning dummy data");

                return new UserDTO(
                        PersonName: "Temporarily Unavailable (timeout)",
                        Email: "Temporarily Unavailable (timeout)",
                        Gender: "Temporarily Unavailable (timeout)",
                        UserID: Guid.Empty);
            }


        }
    }
}
