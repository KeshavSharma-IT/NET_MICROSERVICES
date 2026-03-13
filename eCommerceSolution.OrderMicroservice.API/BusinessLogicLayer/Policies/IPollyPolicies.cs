
using Polly;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Policies
{
    public interface IPollyPolicies
    {

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount);
        IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak);
        IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeout);

    }
}
