using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace eCommerce.OrderMicroservice.BusinessLogicLayer.Policies
{
    public interface IProductsMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetFallBackPolicy();
        IAsyncPolicy<HttpResponseMessage> GetBulkheadIsolationPolicyPolicy();
    }
}
