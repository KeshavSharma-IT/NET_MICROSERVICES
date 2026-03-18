
using eCommerce.OrderMicroservice.BusinessLogicLayer.DTO;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net.Http.Json;
using System.Text.Json;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.HttpClients
{
    public class UserMicroserviceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _distributedCache;

        private readonly ILogger<UserMicroserviceClient> _logger;
        public UserMicroserviceClient(HttpClient httpClient, ILogger<UserMicroserviceClient> logger, IDistributedCache distributedCache)
        {
             _httpClient = httpClient;
            _logger = logger;
            _distributedCache = distributedCache;
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
                //using cache before hitting api
                string cacheKey = $"user:{userID}";
                string? cachedUser = await _distributedCache.GetStringAsync(cacheKey);

                if (cachedUser != null)
                {
                    UserDTO? cacheduser = JsonSerializer.Deserialize<UserDTO>(cachedUser);
                    return cacheduser;
                }

                // if data not found hitting api

                HttpResponseMessage response = await _httpClient.GetAsync($"/gateway/users/{userID}");   

                if (!response.IsSuccessStatusCode)
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        UserDTO? userFromFallBack = await response.Content.ReadFromJsonAsync<UserDTO?>();

                        if (userFromFallBack == null)
                        {

                            throw new NotImplementedException("fallBack policy was not implemented");
                        }
                        return userFromFallBack;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
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

                // adding product in cache
                string userJson = JsonSerializer.Serialize(user);

                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                     .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                     .SetSlidingExpiration(TimeSpan.FromSeconds(100));

                string cacheKeyToWrite = $"user:{userID}";
                await _distributedCache.SetStringAsync(cacheKeyToWrite, userJson, options);


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
