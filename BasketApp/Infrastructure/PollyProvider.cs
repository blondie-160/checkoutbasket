using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace BasketApp.Infrastructure
{
    public interface IPollyProvider
    {
        AsyncRetryPolicy BasketStorePolicy();
    }

    public class PollyProvider : IPollyProvider
    {
        public AsyncRetryPolicy BasketStorePolicy()
        {
            return Policy
               .Handle<Exception>()
                .WaitAndRetryAsync(3, x => TimeSpan.FromMilliseconds(1000));
        }
    }
}
