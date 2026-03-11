
using Polly;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Policies
{
    public interface  IUserMicroServicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy();
        IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy();
        IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy();
        IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();

    }
}
