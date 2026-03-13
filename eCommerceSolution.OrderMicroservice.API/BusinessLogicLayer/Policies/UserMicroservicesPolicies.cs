using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Policies
{
    public class UserMicroservicesPolicies : IUserMicroServicePolicies
    {
        private readonly ILogger<UserMicroservicesPolicies> _logger;
        private readonly IPollyPolicies _pollyPolicies;

        public UserMicroservicesPolicies(ILogger<UserMicroservicesPolicies> logger, IPollyPolicies pollyPolicies)
        {
             _logger = logger;
            _pollyPolicies = pollyPolicies;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var retryPolicy = _pollyPolicies.GetRetryPolicy(5);
            var circuitBreakerPolicy = _pollyPolicies.GetCircuitBreakerPolicy(3,TimeSpan.FromMinutes(2));
            var timeoutPolicy = _pollyPolicies.GetTimeoutPolicy(TimeSpan.FromMinutes(2));

            AsyncPolicyWrap<HttpResponseMessage> wrappedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
            return wrappedPolicy;
        }
    }
}
