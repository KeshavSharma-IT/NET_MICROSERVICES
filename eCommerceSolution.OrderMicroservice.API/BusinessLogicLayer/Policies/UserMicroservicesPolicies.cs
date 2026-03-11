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

        public UserMicroservicesPolicies(ILogger<UserMicroservicesPolicies> logger)
        {
             _logger = logger;
        }


        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            AsyncRetryPolicy<HttpResponseMessage> policy=
            Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
               .WaitAndRetryAsync(retryCount: 5,  //number of time retry
               //sleepDurationProvider: retryAttermp => TimeSpan.FromSeconds(2),  sending requestion ever 2 second now 
               sleepDurationProvider: retryAttermp => TimeSpan.FromSeconds(Math.Pow(2,retryAttermp)), // increaing retry time
               onRetry: (outcome, timespan, retryAttempt, context) =>
               {
                   _logger.LogInformation($"Retry {retryAttempt} after {timespan.TotalSeconds}");
               });

            return policy;
        }
        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            AsyncCircuitBreakerPolicy<HttpResponseMessage> policy =
           Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
           .CircuitBreakerAsync(handledEventsAllowedBeforeBreaking: 3, // no of retry
           durationOfBreak: TimeSpan.FromMinutes(2),         // delay between
           onBreak: (outcome, timespan) =>
               {
                   _logger.LogInformation($"Circuit Breaker open for {timespan.TotalSeconds} due to consecuitivr 3 failure. The subsequent request will be blocked");
               }
               ,
            onReset: ()=>{
                _logger.LogInformation($"Circuit Breaker closed. The subsequent request will be allowed");
            }
           );                                



            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
           AsyncTimeoutPolicy<HttpResponseMessage> policy= Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromMilliseconds(1500));
            return policy;

        }

        public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
        {
            var retryPolicy = GetRetryPolicy();
            var circuitBreakerPolicy = GetCircuitBreakerPolicy();
            var timeoutPolicy = GetTimeoutPolicy();

            AsyncPolicyWrap<HttpResponseMessage> wrappedPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
            return wrappedPolicy;
        }
    }
}
