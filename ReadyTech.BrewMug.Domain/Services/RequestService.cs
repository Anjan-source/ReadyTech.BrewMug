using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTech.BrewMug.Domain.Services
{
    /// <summary>
    /// Request Service to maitain the request count and intcrement atomically and utc time
    /// </summary>
    public class RequestService
    {
        private int _requestCount;

        public int RequestCount
        {
            get { return _requestCount; }
        }

        public void IncrementRequestCount()
        {
            Interlocked.Increment(ref _requestCount);
        }

        public int GetRequestCount()
        {
            return _requestCount;
        }

        public void ResetRequestCount()
        {
            Interlocked.Exchange(ref _requestCount, 0);
        }
        public DateTimeOffset UtcDateTime { get; set; } = DateTimeOffset.UtcNow;
    }
}
